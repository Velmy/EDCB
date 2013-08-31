using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CtrlCmdCLI;
using CtrlCmdCLI.Def;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Threading;
using System.Windows.Controls.Primitives;

namespace EpgTimer.TunerReserveViewCtrl
{
    /// <summary>
    /// TunerReserveView.xaml の相互作用ロジック
    /// </summary>
    public partial class TunerReserveView : UserControl
    {
        public delegate void ProgramViewClickHandler(object sender, Point cursorPos);
        public event ScrollChangedEventHandler ScrollChanged = null;
        public event ProgramViewClickHandler LeftDoubleClick = null;
        public event ProgramViewClickHandler RightClick = null;
        private List<Rectangle> reserveBorder = new List<Rectangle>();

        private Point lastDownMousePos;
        private double lastDownHOffset;
        private double lastDownVOffset;
        private bool isDrag = false;

        private Point lastPopupPos;
        private ReserveViewItem lastPopupInfo = null;

        public TunerReserveView()
        {
            InitializeComponent();
        }

        public void ClearInfo()
        {
            lastPopupInfo = null;
            popupItem.Visibility = System.Windows.Visibility.Hidden;

            foreach (Rectangle info in reserveBorder)
            {
                canvas.Children.Remove(info);
            }
            reserveBorder.Clear();

            reserveViewPanel.ReleaseMouseCapture();
            isDrag = false;

            reserveViewPanel.Items = null;
            reserveViewPanel.Height = 0;
            reserveViewPanel.Width = 0;
            canvas.Height = 0;
            canvas.Width = 0;
        }

        public void SetReserveList(List<ReserveViewItem> programList, double width, double height)
        {
            try
            {
                canvas.Height = Math.Ceiling(height);
                canvas.Width = Math.Ceiling(width);
                reserveViewPanel.Height = Math.Ceiling(height);
                reserveViewPanel.Width = Math.Ceiling(width);
                reserveViewPanel.Items = programList;
                reserveViewPanel.InvalidateVisual();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        private void reserveViewPanel_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (sender.GetType() == typeof(TunerReservePanel))
                {
                    if (e.LeftButton == MouseButtonState.Pressed && isDrag == true)
                    {
                        Point CursorPos = Mouse.GetPosition(null);
                        double MoveX = lastDownMousePos.X - CursorPos.X;
                        double MoveY = lastDownMousePos.Y - CursorPos.Y;

                        double OffsetH = 0;
                        double OffsetV = 0;
                        MoveX *= Settings.Instance.DragScroll;
                        MoveY *= Settings.Instance.DragScroll;
                        OffsetH = lastDownHOffset + MoveX;
                        OffsetV = lastDownVOffset + MoveY;
                        if (OffsetH < 0)
                        {
                            OffsetH = 0;
                        }
                        if (OffsetV < 0)
                        {
                            OffsetV = 0;
                        }

                        scrollViewer.ScrollToHorizontalOffset(Math.Floor(OffsetH));
                        scrollViewer.ScrollToVerticalOffset(Math.Floor(OffsetV));
                    }
                    else
                    {
                        Point CursorPos = Mouse.GetPosition(reserveViewPanel);
                        if (lastPopupPos != CursorPos)
                        {
                            PopupItem();
                            lastPopupPos = CursorPos;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        private void reserveViewPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                lastDownMousePos = Mouse.GetPosition(null);
                lastDownHOffset = scrollViewer.HorizontalOffset;
                lastDownVOffset = scrollViewer.VerticalOffset;
                reserveViewPanel.CaptureMouse();
                isDrag = true;

                if (e.ClickCount == 2)
                {
                    Point cursorPos = Mouse.GetPosition(reserveViewPanel);
                    if (LeftDoubleClick != null)
                    {
                        LeftDoubleClick(sender, cursorPos);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        private void reserveViewPanel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                reserveViewPanel.ReleaseMouseCapture();
                isDrag = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        private void scrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (ScrollChanged != null)
            {
                scrollViewer.ScrollToHorizontalOffset(Math.Floor(scrollViewer.HorizontalOffset));
                scrollViewer.ScrollToVerticalOffset(Math.Floor(scrollViewer.VerticalOffset));
                ScrollChanged(this, e);
            }
        }

        private void reserveViewPanel_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            reserveViewPanel.ReleaseMouseCapture();
            isDrag = false;
            lastDownMousePos = Mouse.GetPosition(null);
            lastDownHOffset = scrollViewer.HorizontalOffset;
            lastDownVOffset = scrollViewer.VerticalOffset;
            if (e.ClickCount == 1)
            {
                Point cursorPos = Mouse.GetPosition(reserveViewPanel);
                if (RightClick != null)
                {
                    RightClick(sender, cursorPos);
                }
            }
        }

        private void reserveViewPanel_MouseLeave(object sender, MouseEventArgs e)
        {
            popupItem.Visibility = System.Windows.Visibility.Hidden;
            lastPopupInfo = null;
            lastPopupPos = new Point(-1, -1);
        }

        protected void PopupItem()
        {
            ReserveViewItem info = null;

            if (reserveViewPanel.Items != null)
            {
                Point cursorPos2 = Mouse.GetPosition(scrollViewer);
                if (cursorPos2.X < 0 || cursorPos2.Y < 0 ||
                    scrollViewer.ViewportWidth < cursorPos2.X || scrollViewer.ViewportHeight < cursorPos2.Y)
                {
                    return;
                }
                Point cursorPos = Mouse.GetPosition(reserveViewPanel);
                foreach (ReserveViewItem item in reserveViewPanel.Items)
                {
                    if (item.LeftPos <= cursorPos.X && cursorPos.X < item.LeftPos + item.Width)
                    {
                        if (item.TopPos <= cursorPos.Y && cursorPos.Y < item.TopPos + item.Height)
                        {
                            if (item == lastPopupInfo) return;

                            info = item;
                            lastPopupInfo = info;
                            lastPopupPos = cursorPos;
                            break;
                        }
                    }
                }
            }

            if (info == null || info.ReserveInfo == null)
            {
                popupItem.Visibility = System.Windows.Visibility.Hidden;
                lastPopupInfo = null;
                return;
            }

            double sizeNormal = Settings.Instance.FontSize;
            double sizeTitle = Settings.Instance.FontSizeTitle;

            FontFamily fontNormal = null;
            FontFamily fontTitle = null;
            try
            {
                if (Settings.Instance.FontName.Length > 0)
                {
                    fontNormal = new FontFamily(Settings.Instance.FontName);
                }
                if (Settings.Instance.FontNameTitle.Length > 0)
                {
                    fontTitle = new FontFamily(Settings.Instance.FontNameTitle);
                }

                if (fontNormal == null)
                {
                    fontNormal = new FontFamily("MS UI Gothic");
                }
                if (fontTitle == null)
                {
                    fontTitle = new FontFamily("MS UI Gothic");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace);
            }

            ReserveItem reserveItem = new ReserveItem(info.ReserveInfo);
            popupItem.Background = reserveItem.BackColor;

            Canvas.SetTop(popupItem, info.TopPos);
            popupItem.MinHeight = info.Height;
            Canvas.SetLeft(popupItem, info.LeftPos);
            popupItem.Width = info.Width;

            if (info.ReserveInfo.OverlapMode == 2)
            {
                errText.Text = reserveItem.Comment;
                errText.FontFamily = fontTitle;
                errText.FontSize = sizeTitle;
                errText.FontWeight = FontWeights.Bold;
                errText.Foreground = CommonManager.Instance.CustTitle1Color;
                errText.Margin = new Thickness(1, 1, 1, sizeTitle / 2);
                errText.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                errText.Text = "";
                errText.Height = 0;
                errText.Margin = new Thickness();
                errText.Visibility = System.Windows.Visibility.Hidden;
            }

            String text;
            DateTime endTime = info.ReserveInfo.StartTime + TimeSpan.FromSeconds(info.ReserveInfo.DurationSecond);
            text = info.ReserveInfo.StartTime.ToString("HH:mm ～ ");
            text += endTime.ToString("HH:mm");
            minText.Text = text;
            minText.FontFamily = fontNormal;// fontTitle;
            minText.FontSize = sizeNormal;
            minText.FontWeight = FontWeights.Normal;
            minText.Foreground = CommonManager.Instance.CustTitle1Color;
            minText.Margin = new Thickness(1, 1, 1, 1);

            //minGrid.Width = new GridLength(sizeNormal * 1.7 + 1);

            double indent = Settings.Instance.EpgTitleIndent ? sizeNormal * 2 : 2;
            text = reserveItem.ServiceName;
            text += " (" + reserveItem.NetworkName + ")";
            titleText.Text = text;
            titleText.FontFamily = fontTitle;
            titleText.FontSize = sizeTitle;
            titleText.FontWeight = FontWeights.Bold;
            titleText.Foreground = CommonManager.Instance.CustTitle1Color;
            titleText.Margin = new Thickness(indent, 1, 1, sizeNormal / 2);

            infoText.Text = reserveItem.EventName;
            infoText.FontFamily = fontNormal;
            infoText.FontSize = sizeNormal;
            infoText.FontWeight = FontWeights.Normal;
            infoText.Foreground = CommonManager.Instance.CustTitle2Color;
            infoText.Margin = new Thickness(indent, 0, 9.5, sizeNormal / 2);

            popupItem.Visibility = System.Windows.Visibility.Visible;
        }
    }
}

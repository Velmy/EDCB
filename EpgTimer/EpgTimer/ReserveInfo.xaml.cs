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

namespace EpgTimer
{
    /// <summary>
    /// ReserveInfo.xaml の相互作用ロジック
    /// </summary>
    public partial class ReserveInfo : UserControl
    {
        public ReserveInfo()
        {
            InitializeComponent();
        }

        public void SaveSize()
        {
            reserveView.SaveSize();
            epgAutoAddView.SaveSize();
            manualAutoAddView.SaveSize();
        }

        /// <summary>
        /// 予約情報の更新通知
        /// </summary>
        public void UpdateReserveData()
        {
            reserveView.UpdateReserveData();
            tunerReserveView.UpdateReserveData();
        }
        public void UpdateAutoAddInfo()
        {
            epgAutoAddView.UpdateInfo();
            manualAutoAddView.UpdateInfo();
        }
    }
}

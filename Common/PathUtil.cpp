#include "stdafx.h"
#include "PathUtil.h"
#include "StringUtil.h"
#include <shlobj.h>
#include <shlwapi.h>
#include <Lm.h>

#pragma comment(lib, "Netapi32.lib")

void GetDefSettingPath(wstring& strPath)
{
/*	strPath = L"";

	WCHAR szPathM[_MAX_PATH];
	::SHGetFolderPath(NULL, CSIDL_PERSONAL|CSIDL_FLAG_CREATE, NULL, 0, szPathM);

	strPath = szPathM;
	strPath += SAVE_FOLDER;
	ChkFolderPath(strPath);
*/
	WCHAR strExePath[512] = L"";
	GetModuleFileName(NULL, strExePath, 512);

	WCHAR szPath[_MAX_PATH];	// パス
	WCHAR szDrive[_MAX_DRIVE];
	WCHAR szDir[_MAX_DIR];
	WCHAR szFname[_MAX_FNAME];
	WCHAR szExt[_MAX_EXT];
	_wsplitpath_s( strExePath, szDrive, _MAX_DRIVE, szDir, _MAX_DIR, szFname, _MAX_FNAME, szExt, _MAX_EXT );
	_wmakepath_s(  szPath, _MAX_PATH, szDrive, szDir, NULL, NULL );
	
	strPath = L"";
	strPath += szPath;
	strPath += L"Setting";
}

void GetSettingPath(wstring& strPath)
{
	strPath = L"";
	wstring strIni = L"";
	GetCommonIniPath(strIni);
	
	WCHAR wPath[512]=L"";
	GetPrivateProfileString( L"Set", L"DataSavePath", L"", wPath, 512, strIni.c_str() );
	strPath = wPath;
	if( strPath.empty() == true ){
		GetDefSettingPath(strPath);
	}
	ChkFolderPath(strPath);
}

void GetModuleFolderPath(wstring& strPath)
{
	WCHAR strExePath[512] = L"";
	GetModuleFileName(NULL, strExePath, 512);

	WCHAR szPath[_MAX_PATH];	// パス
	WCHAR szDrive[_MAX_DRIVE];
	WCHAR szDir[_MAX_DIR];
	WCHAR szFname[_MAX_FNAME];
	WCHAR szExt[_MAX_EXT];
	_wsplitpath_s( strExePath, szDrive, _MAX_DRIVE, szDir, _MAX_DIR, szFname, _MAX_FNAME, szExt, _MAX_EXT );
	_wmakepath_s(  szPath, _MAX_PATH, szDrive, szDir, NULL, NULL );
	int iLen = lstrlen(szPath);
	szPath[iLen-1] = '\0';
	
	strPath = L"";
	strPath += szPath;
	ChkFolderPath(strPath);
}

void GetModuleIniPath(wstring& strPath)
{
	WCHAR strExePath[512] = L"";
	GetModuleFileName(NULL, strExePath, 512);

	WCHAR szPath[_MAX_PATH];	// パス
	WCHAR szDrive[_MAX_DRIVE];
	WCHAR szDir[_MAX_DIR];
	WCHAR szFname[_MAX_FNAME];
	WCHAR szExt[_MAX_EXT];
	_wsplitpath_s( strExePath, szDrive, _MAX_DRIVE, szDir, _MAX_DIR, szFname, _MAX_FNAME, szExt, _MAX_EXT );
	_wmakepath_s(  szPath, _MAX_PATH, szDrive, szDir, NULL, NULL );
	
	strPath = L"";
	strPath += szPath;
	strPath += szFname;
	strPath += L".ini";
}

void GetCommonIniPath(wstring& strPath)
{
	WCHAR strExePath[512] = L"";
	GetModuleFileName(NULL, strExePath, 512);

	WCHAR szPath[_MAX_PATH];	// パス
	WCHAR szDrive[_MAX_DRIVE];
	WCHAR szDir[_MAX_DIR];
	WCHAR szFname[_MAX_FNAME];
	WCHAR szExt[_MAX_EXT];
	_wsplitpath_s( strExePath, szDrive, _MAX_DRIVE, szDir, _MAX_DIR, szFname, _MAX_FNAME, szExt, _MAX_EXT );
	_wmakepath_s(  szPath, _MAX_PATH, szDrive, szDir, NULL, NULL );
	
	strPath = L"";
	strPath += szPath;
	strPath += L"Common.ini";
}

void GetEpgTimerSrvIniPath(wstring& strPath)
{
	WCHAR strExePath[512] = L"";
	GetModuleFileName(NULL, strExePath, 512);

	WCHAR szPath[_MAX_PATH];	// パス
	WCHAR szDrive[_MAX_DRIVE];
	WCHAR szDir[_MAX_DIR];
	WCHAR szFname[_MAX_FNAME];
	WCHAR szExt[_MAX_EXT];
	_wsplitpath_s( strExePath, szDrive, _MAX_DRIVE, szDir, _MAX_DIR, szFname, _MAX_FNAME, szExt, _MAX_EXT );
	_wmakepath_s(  szPath, _MAX_PATH, szDrive, szDir, NULL, NULL );
	
	strPath = L"";
	strPath += szPath;
	strPath += L"EpgTimerSrv.ini";
}

void GetEpgSavePath(wstring& strPath)
{
	strPath = L"";
	GetSettingPath(strPath);
	strPath += EPG_SAVE_FOLDER;
}

void GetLogoSavePath(wstring& strPath)
{
	strPath = L"";
	GetSettingPath(strPath);
	strPath += LOGO_SAVE_FOLDER;
}

void GetRecFolderPath(wstring& strPath)
{
	strPath = L"";
	wstring strIni = L"";
	GetCommonIniPath(strIni);
	
	WCHAR wPath[512]=L"";
	GetPrivateProfileString( L"Set", L"RecFolderPath0", L"", wPath, 512, strIni.c_str() );
	strPath = wPath;
	if( strPath.empty() == true ){
		GetDefSettingPath(strPath);
	}
	ChkFolderPath(strPath);
}

void GetFileTitle(wstring strPath, wstring& strTitle)
{
	WCHAR szDrive[_MAX_DRIVE];
	WCHAR szDir[_MAX_DIR];
	WCHAR szFname[_MAX_FNAME];
	WCHAR szExt[_MAX_EXT];
	_wsplitpath_s( strPath.c_str(), szDrive, _MAX_DRIVE, szDir, _MAX_DIR, szFname, _MAX_FNAME, szExt, _MAX_EXT );

	strTitle = szFname;
	return ;
}

void GetFileName(wstring strPath, wstring& strName)
{
	WCHAR strFileName[512] = L"";
	WCHAR szDrive[_MAX_DRIVE];
	WCHAR szDir[_MAX_DIR];
	WCHAR szFname[_MAX_FNAME];
	WCHAR szExt[_MAX_EXT];
	_wsplitpath_s( strPath.c_str(), szDrive, _MAX_DRIVE, szDir, _MAX_DIR, szFname, _MAX_FNAME, szExt, _MAX_EXT );
	_wmakepath_s( strFileName, _MAX_PATH, NULL, NULL, szFname, szExt );

	strName = strFileName;
	return ;
}

void GetFileExt(wstring strPath, wstring& strExt)
{
	WCHAR strFileName[512] = L"";
	WCHAR szDrive[_MAX_DRIVE];
	WCHAR szDir[_MAX_DIR];
	WCHAR szFname[_MAX_FNAME];
	WCHAR szExt[_MAX_EXT];
	_wsplitpath_s( strPath.c_str(), szDrive, _MAX_DRIVE, szDir, _MAX_DIR, szFname, _MAX_FNAME, szExt, _MAX_EXT );

	strExt = szExt;
	return ;
}

void GetFileFolder(wstring strPath, wstring& strFolder)
{
	WCHAR szPath[512] = L"";
	WCHAR szDrive[_MAX_DRIVE];
	WCHAR szDir[_MAX_DIR];
	WCHAR szFname[_MAX_FNAME];
	WCHAR szExt[_MAX_EXT];
	_wsplitpath_s( strPath.c_str(), szDrive, _MAX_DRIVE, szDir, _MAX_DIR, szFname, _MAX_FNAME, szExt, _MAX_EXT );
	_wmakepath_s( szPath, _MAX_PATH, szDrive, szDir, NULL, NULL );

	strFolder = szPath;
	ChkFolderPath(strFolder);
	return ;
}


BOOL IsExt(wstring filePath, wstring ext)
{
	if( filePath.empty() == true ){
		return FALSE;
	}
	WCHAR szDrive[_MAX_DRIVE];
	WCHAR szDir[_MAX_DIR];
	WCHAR szFname[_MAX_FNAME];
	WCHAR szExt[_MAX_EXT];
	_wsplitpath_s( filePath.c_str(), szDrive, _MAX_DRIVE, szDir, _MAX_DIR, szFname, _MAX_FNAME, szExt, _MAX_EXT );

	if( CompareNoCase( szExt, ext ) != 0 ){
		return FALSE;
	}

	return TRUE;
}

void CheckFileName(wstring& fileName, BOOL noChkYen)
{
	if( noChkYen == FALSE ){
		Replace(fileName, L"\\",L"￥");
	}
	Replace(fileName, L"/",L"／");
	Replace(fileName, L":",L"：");
	Replace(fileName, L"*",L"＊");
	Replace(fileName, L"?",L"？");
	Replace(fileName, L"\"",L"”");
	Replace(fileName, L"<",L"＜");
	Replace(fileName, L">",L"＞");
	Replace(fileName, L"|",L"｜");
}

void CheckFileName(string& fileName, BOOL noChkYen)
{
	if( noChkYen == FALSE ){
		Replace(fileName, "\\","￥");
	}
	Replace(fileName, "/","／");
	Replace(fileName, ":","：");
	Replace(fileName, "*","＊");
	Replace(fileName, "?","？");
	Replace(fileName, "\"","”");
	Replace(fileName, "<","＜");
	Replace(fileName, ">","＞");
	Replace(fileName, "|","｜");
}

BOOL GetNetworkPath(const wstring strPath, wstring& strNetPath)
{
	TCHAR relative[MAX_PATH] = _T("");
	TCHAR netname[MAX_PATH] = _T("");

	NET_API_STATUS res;
	do
	{
		PSHARE_INFO_502 BufPtr,p;
		DWORD er=0, tr=0, resume=0;
		res = NetShareEnum(NULL, 502, (LPBYTE *)&BufPtr, -1, &er, &tr, &resume);
		if (res == ERROR_SUCCESS || res == ERROR_MORE_DATA)
		{
			p = BufPtr;
			for (DWORD i = 1; i <= er; i++)
			{
				// 共有名が$で終わるのは隠し共有
				if (p->shi502_netname[lstrlen(p->shi502_netname)-1] != _T('$'))
				{
					if (PathIsDirectory(p->shi502_path))
					{
						TCHAR tmp[MAX_PATH];
						if (PathRelativePathTo(tmp, p->shi502_path, FILE_ATTRIBUTE_DIRECTORY, strPath.c_str(), 0))
						{
							if (lstrlen(relative) == 0 || lstrlen(relative) > lstrlen(tmp))
							{
								lstrcpy(relative, tmp);
								lstrcpy(netname, p->shi502_netname);
							}
						}
					}
				}
				p++;
			}
			NetApiBufferFree(BufPtr);
		}
	} while (res==ERROR_MORE_DATA);

	if (lstrlen(relative) == 0) return FALSE;

	TCHAR name[MAX_COMPUTERNAME_LENGTH+1];
	DWORD len = MAX_COMPUTERNAME_LENGTH + 1;
	if (!GetComputerName(name, &len)) return FALSE;

	TCHAR result[MAX_PATH];
	wsprintf(result, _T("\\\\%s\\%s"), name, netname);
	lstrcat(result, &relative[1]);

	strNetPath = result;

	return TRUE;
}

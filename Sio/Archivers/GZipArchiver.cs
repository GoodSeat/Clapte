using System;
using System.Collections.Generic;
using System.Text;

namespace GoodSeat.Sio.Archivers
{
	/// <summary>
	/// GZip形式によるファイル圧縮/解凍操作を提供します。
	/// </summary>
	public static class GZipArchiver
	{
		/// <summary>
		/// 指定ファイルをGZip形式で圧縮して保存します。
		/// </summary>
		/// <param name="baseFilePath">保存対象のファイルパス</param>
		/// <param name="compressedFilePath">保存先ファイルパス</param>
		public static void Compress(string baseFilePath, string compressedFilePath)
		{
			//作成する圧縮ファイルのFileStreamを作成する
			System.IO.FileStream compFileStrm =
				new System.IO.FileStream(compressedFilePath, System.IO.FileMode.Create);
			//圧縮モードのGZipStreamを作成する
			System.IO.Compression.GZipStream gzipStrm =
				new System.IO.Compression.GZipStream(compFileStrm,
					System.IO.Compression.CompressionMode.Compress);

			//圧縮するファイルを開いて少しずつbufferに読み込む
			System.IO.FileStream inFileStrm = new System.IO.FileStream(
				baseFilePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
			byte[] buffer = new byte[1024];
			while (true)
			{
				//圧縮するファイルからデータを読み込む
				int readSize = inFileStrm.Read(buffer, 0, buffer.Length);
				//最後まで読み込んだ時は、ループを抜ける
				if (readSize == 0)
					break;
				//データを圧縮して書き込む
				gzipStrm.Write(buffer, 0, readSize);
			}

			//閉じる
			inFileStrm.Close();
			gzipStrm.Close();
		}


		/// <summary>
		/// 指定したGZip形式のファイルを解凍します。
		/// </summary>
		/// <param name="compressedFilePath">解凍対象のGZipファイルパス</param>
		/// <param name="decompressedFilePath">解凍先のファイルパス</param>
		public static void Decompress(string compressedFilePath, string decompressedFilePath)
		{
			//展開する書庫のFileStreamを作成する
			System.IO.FileStream gzipFileStrm = new System.IO.FileStream(
				compressedFilePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
			//圧縮解除モードのGZipStreamを作成する
			System.IO.Compression.GZipStream gzipStrm =
				new System.IO.Compression.GZipStream(gzipFileStrm,
					System.IO.Compression.CompressionMode.Decompress);
			//展開先のファイルのFileStreamを作成する
			System.IO.FileStream outFileStrm = new System.IO.FileStream(
				decompressedFilePath, System.IO.FileMode.Create, System.IO.FileAccess.Write);

			byte[] buffer = new byte[1024];
			while (true)
			{
				//書庫から展開されたデータを読み込む
				int readSize = gzipStrm.Read(buffer, 0, buffer.Length);
				//最後まで読み込んだ時は、ループを抜ける
				if (readSize == 0)
					break;
				//展開先のファイルに書き込む
				outFileStrm.Write(buffer, 0, readSize);
			}

			//閉じる
			outFileStrm.Close();
			gzipStrm.Close();
		}
	}
}

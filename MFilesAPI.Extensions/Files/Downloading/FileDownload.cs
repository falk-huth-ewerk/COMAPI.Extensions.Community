﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MFilesAPI;

namespace MFilesAPI.Extensions
{
	/// <summary>
	/// A file downloaded from the M-Files server to a temporary file on disk.
	/// </summary>
	public class FileDownload
		: DisposableBase
	{
		/// <summary>
		/// The block size to use for downloads, if none is specified.
		/// </summary>
		public const int DefaultDownloadBlockSize = 4096;

		/// <summary>
		/// The temporary file to download to.
		/// Populated by calling <see cref="FileDownload.DownloadFile(bool, int, MFFileFormat)"/>.
		/// </summary>
		public FileInfo TargetFile { get; protected set; }

		/// <summary>
		/// The vault to download from.
		/// </summary>
		public Vault Vault { get; protected set; }

		/// <summary>
		/// The file to download.
		/// </summary>
		public ObjectFile FileToDownload { get; protected set; }

		/// <summary>
		/// Represents a file to be downloaded from the vault to disk.
		/// Only actually downloaded when <see cref="FileDownload.DownloadFile(bool, int, MFFileFormat)"/> is called.
		/// </summary>
		/// <param name="fileToDownload">The file to download.</param>
		/// <param name="vault">The vault to download from.</param>
		/// <param name="targetFile">The <see cref="FileInfo"/> to download the file to.</param>
		protected FileDownload
		(
			ObjectFile fileToDownload,
			Vault vault,
			FileInfo targetFile
		)
		{
			// Set properties.
			this.FileToDownload = fileToDownload ?? throw new ArgumentNullException(nameof(fileToDownload));
			this.Vault = vault ?? throw new ArgumentNullException(nameof(vault));
			this.TargetFile = targetFile ?? throw new ArgumentNullException(nameof(targetFile));
		}

		/// <summary>
		/// Downloads the file from the vault.
		/// </summary>
		/// <param name="overwriteExistingFiles">
		/// If the <see cref="FileDownload.TargetFile"/> exists and this is true then it will be deleted prior to download starting.
		/// If it exists and this is false then an <see cref="InvalidOperationException"/> is thrown. </param>
		/// <param name="blockSize">The size of blocks to use to transfer the file from the M-Files vault to this machine.</param>
		/// <param name="fileFormat">The format of file to request from server.</param>
		public virtual void DownloadFile
		(
			bool overwriteExistingFiles = true,
			int blockSize = FileDownload.DefaultDownloadBlockSize,
			MFFileFormat fileFormat = MFFileFormat.MFFileFormatNative
		)
		{
			// If the target file exists and overwriteExistingFiles is false then throw.
			if (this.TargetFile.Exists)
			{
				if (false == overwriteExistingFiles)
				{
					throw new InvalidOperationException("File already exists");
				}

				// Exists, but we can overwrite it.
				this.TargetFile.Delete();
			}

			// Open the target file for writing.
			using (var stream = this.TargetFile.OpenWrite())
			{
				// Start the download session.
				var downloadSession = this
					.Vault
					.ObjectFileOperations
					.DownloadFileInBlocks_BeginEx
					(
						this.FileToDownload.ID,
						this.FileToDownload.Version,
						fileFormat
					);

				// Write out each block in turn.
				long offset = 0;
				while (offset < downloadSession.FileSize)
				{
					// Get the block.
					var data = this
						.Vault
						.ObjectFileOperations
						.DownloadFileInBlocks_ReadBlock
						(
							downloadSession.DownloadID,
							blockSize,
							offset
						);

					// Write the block to disk.
					stream.Write(data, 0, data.Length);

					// Increment offset.
					offset += data.Length;

					// Flush the data to disk.
					stream.Flush();
				}

				// Finish the download session.
				this.Vault.ObjectFileOperations.CancelFileDownloadSession(downloadSession.DownloadID);
			}

			// Re-hydrate the temporary file data.
			this.TargetFile = new FileInfo(this.TargetFile.FullName);
		}

		/// <inheritdoc />
		protected override void DisposeManagedObjects()
		{
			// If the target file exists then delete it.
			if (this.TargetFile?.Exists == true)
			{
				this.TargetFile.Delete();
			}
		}

		/// <summary>
		/// Creates a <see cref="FileDownload"/> and downloads <paramref name="fileToDownload"/>
		/// from the <paramref name="vault"/> to <paramref name="downloadTo"/>.
		/// </summary>
		/// <param name="fileToDownload">The file to download.</param>
		/// <param name="vault">The vault to download from.</param>
		/// <param name="downloadTo">The location on disk to download to.</param>
		/// <param name="overwriteExistingFiles">If false and <paramref name="downloadTo"/> exists then an <see cref="InvalidOperationException"/> is thrown.</param>
		/// <param name="blockSize">The size of the block to use for file transfers (defaults to <see cref="FileDownload.DefaultDownloadBlockSize"/>).</param>
		/// <param name="fileFormat">The format to request the file in from the server.</param>
		/// <returns></returns>
		public static FileDownload DownloadFile
		(
			ObjectFile fileToDownload,
			Vault vault,
			FileInfo downloadTo,
			bool overwriteExistingFiles = true,
			int blockSize = FileDownload.DefaultDownloadBlockSize,
			MFFileFormat fileFormat = MFFileFormat.MFFileFormatNative
		)
		{
			// Create the download object.
			var download = new FileDownload
			(
				fileToDownload,
				vault,
				downloadTo
			);

			// Download the file.
			download.DownloadFile(overwriteExistingFiles, blockSize, fileFormat);

			// Return the download object.
			return download;
		}
	}
}
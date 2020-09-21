using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;

namespace excel_test
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		string sFileName;
		List<paya_inf> payainfList = new List<paya_inf>();
		List<havale_inf> havaleinfList = new List<havale_inf>();

		string fileFullName = string.Empty;

		Excel.Application xlApp;
		Excel.Workbook xlWorkBook;
		Excel.Worksheet xlWorkSheet;
		Excel.Range range;

		public void closeFile()
		{
			xlWorkBook.Close(true, null, null);
			xlApp.Quit();
		}
		private void button1_Click(object sender, EventArgs e)
		{
			payainfList.Clear();
			labelDone.Text = "-";
			labelPaya.Text = "-";
			labelHavale.Text = "-";
			try
			{
				int rw = 0;

				openFileDialog1.Title = "Excel File to Edit";
				openFileDialog1.FileName = "";
				openFileDialog1.Filter = "Excel File|*.xlsx;*.xls";
				if (openFileDialog1.ShowDialog() == DialogResult.OK)
				{
					sFileName = openFileDialog1.FileName;

					xlApp = new Excel.Application();
					xlWorkBook = xlApp.Workbooks.Open(sFileName);
					xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

					range = xlWorkSheet.UsedRange;
					rw = range.Rows.Count;
					labelPaya.Text = rw.ToString();

					for (int i = 0; i < rw - 1; i++)
					{
						paya_inf inf = new paya_inf();
						inf.rwno = i + 2;
						inf.identifire = (range.Cells[i + 2, 2] as Excel.Range).Text;
						inf.total_paya = (range.Cells[i + 2, 6] as Excel.Range).Text;
						inf.amount_paya = (range.Cells[i + 2, 7] as Excel.Range).Text;

						payainfList.Add(inf);
					}
					closeFile();
				}
			}
			catch (Exception ex)
			{
				closeFile();

				//MessageBox.Show(ex.ToString());
				//const string message = "خطای سیستمی2";
				const string caption = "خطا";
				MessageBox.Show(ex.Message, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			havaleinfList.Clear();
			labelHavale.Text = "-";
			try
			{
				int rw2 = 0;

				openFileDialog2.Title = "Excel File to Edit";
				openFileDialog2.FileName = "";
				openFileDialog2.Filter = "Excel File|*.xlsx;*.xls";
				if (openFileDialog2.ShowDialog() == DialogResult.OK)
				{
					sFileName = openFileDialog2.FileName;

					xlApp = new Excel.Application();
					xlWorkBook = xlApp.Workbooks.Open(sFileName);
					xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

					range = xlWorkSheet.UsedRange;
					rw2 = range.Rows.Count;

					labelHavale.Text = rw2.ToString();
					for (int i = 0; i < rw2 - 1; i++)
					{
						havale_inf inf2 = new havale_inf();
						inf2.identifire = (range.Cells[i + 2, 2] as Excel.Range).Text;
						inf2.total_havale = (range.Cells[i + 2, 5] as Excel.Range).Text;
						inf2.amount_havale = (range.Cells[i + 2, 6] as Excel.Range).Text;

						havaleinfList.Add(inf2);
					}
					closeFile();
				}
			}
			catch (Exception ex)
			{
				closeFile();

				//MessageBox.Show(ex.ToString());
				//const string message = "خطای سیستمی2";
				const string caption = "خطا";
				MessageBox.Show(ex.Message, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			//closeFile();
		}

		private void button3_Click(object sender, EventArgs e)
		{
			labelDone.Text = "Waiting...";
			saveFileDialog1.FileName = "Result";
			saveFileDialog1.Title = "Save Result";
			saveFileDialog1.Filter = "Excel File|*.xlsx";
			try
			{
				if (saveFileDialog1.ShowDialog() == DialogResult.OK)
				{
					fileFullName = saveFileDialog1.FileName;
					
					#region [ create new excel ]
					//if (File.Exists(fileFullName))
					//{

					//}
					//else
					//{

					//}

					xlApp = new Excel.Application();
					object misValue = System.Reflection.Missing.Value;

					xlWorkBook = xlApp.Workbooks.Add(misValue);
					xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

					xlWorkBook.SaveAs(
						Filename: fileFullName,
						FileFormat: Excel.XlFileFormat.xlOpenXMLWorkbook,
						Password: misValue,
						WriteResPassword: misValue,
						ReadOnlyRecommended: false,
						CreateBackup: misValue,
						AccessMode: Excel.XlSaveAsAccessMode.xlNoChange,
						ConflictResolution: misValue,
						AddToMru: misValue,
						TextCodepage: misValue,
						TextVisualLayout: misValue,
						Local: misValue

						);
					closeFile();

					xlWorkBook = xlApp.Workbooks.Open(fileFullName);
					xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
					range = xlWorkSheet.Cells[1, 1].EntireRow;
					range.EntireColumn.NumberFormat = "@";

					#endregion
					range.Cells.Clear();

					xlWorkSheet.Cells[1, 1] = "ID";
					xlWorkSheet.Cells[1, 2] = "Identifire";
					xlWorkSheet.Cells[1, 3] = "Count_Paya";
					xlWorkSheet.Cells[1, 4] = "Count_Havale";
					xlWorkSheet.Cells[1, 5] = "Amount_Paya";
					xlWorkSheet.Cells[1, 6] = "Amount_Havale";
					xlWorkSheet.Cells[1, 7] = "Count";
					xlWorkSheet.Cells[1, 8] = "Amount";
					xlWorkSheet.Cells[1, 9] = "Status";

					xlWorkBook.Save();

					int payaCount = payainfList.Count;

					for (int i = 0; i < payaCount; i++)
					{
						xlWorkSheet.Cells[i + 2, 1] = (i + 1).ToString();
						xlWorkSheet.Cells[i + 2, 2] = payainfList[i].identifire;
						xlWorkSheet.Cells[i + 2, 3] = payainfList[i].total_paya;

						xlWorkSheet.Cells[i + 2, 5] = payainfList[i].amount_paya;

						xlWorkSheet.Cells[i + 2, 7] = "no";
						xlWorkSheet.Cells[i + 2, 8] = "no";
						xlWorkSheet.Cells[i + 2, 9] = "paya";
					}

					xlWorkSheet.Columns.AutoFit();

					//cells["B:B"].NumberFormat = "@";
					xlWorkBook.Save();

					Excel.Range usedRange = xlWorkSheet.UsedRange;
					Excel.Range lastCell = usedRange.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
					int lastRow = lastCell.Row;


					int havaleCount = havaleinfList.Count;

					foreach (var item in havaleinfList)
					{
						bool addDone = false;
						string idenHavale = item.identifire;
						string tHavale = item.total_havale;
						string aHavale = item.amount_havale;

						foreach (var payaItem in payainfList)
						{
							int rwPaya = payaItem.rwno;
							string idenPaya = payaItem.identifire;
							string tPaya = payaItem.total_paya;
							string aPaya = payaItem.amount_paya;

							if (idenHavale == idenPaya)
							{
								xlWorkSheet.Cells[rwPaya, 9] = "both";

								addDone = true;
								xlWorkSheet.Cells[rwPaya, 4] = item.total_havale;
								xlWorkSheet.Cells[rwPaya, 6] = item.amount_havale;
								if (tHavale == tPaya)
								{
									xlWorkSheet.Cells[rwPaya, 7] = "yes";
								}
								if (aHavale == aPaya)
								{
									xlWorkSheet.Cells[rwPaya, 8] = "yes";
								}
								if (tHavale == tPaya && aHavale == aPaya)
								{
									xlWorkSheet.Cells[rwPaya, 9] = "ok";
								}
							}
						}

						if (addDone == false)
						{
							xlWorkSheet.Cells[lastRow + 1, 1] = (lastRow);
							xlWorkSheet.Cells[lastRow + 1, 2] = idenHavale;

							xlWorkSheet.Cells[lastRow + 1, 4] = tHavale;

							xlWorkSheet.Cells[lastRow + 1, 6] = aHavale;
							xlWorkSheet.Cells[lastRow + 1, 7] = "no";
							xlWorkSheet.Cells[lastRow + 1, 8] = "no";
							xlWorkSheet.Cells[lastRow + 1, 9] = "Havale";
							lastRow++;
						}
					}
					xlWorkSheet.Columns.AutoFit();
					xlWorkBook.Save();
					closeFile();
					labelDone.Text = "Done";
				}
				else
				{
					MessageBox.Show("مسیر ذخیره فایل را انتخاب کنید");
				}
			}
			catch (Exception ex)
			{
				closeFile();
				const string caption = "خطا";
				MessageBox.Show(ex.Message, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}
	}
}
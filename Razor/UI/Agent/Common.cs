using RazorEnhanced;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Assistant
{
	public partial class MainForm : System.Windows.Forms.Form
	{
		// ----------------- START AGENT GESTIONE MENU TENDINA -------------------

		private static int agentrowindex = 0;
		private static string agenttype = String.Empty;

		private void datagridMenuStrip_Click(object sender, EventArgs e)
		{
			switch (agenttype)
			{
				case "autolootdataGridView":
					if (!autolootdataGridView.Rows[agentrowindex].IsNewRow)
					{
						autolootdataGridView.Rows.RemoveAt(agentrowindex);
						RazorEnhanced.AutoLoot.CopyTable();
					}
					break;
				case "scavengerdataGridView":
					if (!scavengerdataGridView.Rows[agentrowindex].IsNewRow)
					{
						scavengerdataGridView.Rows.RemoveAt(agentrowindex);
						RazorEnhanced.Scavenger.CopyTable();
					}
					break;
				case "organizerdataGridView":
					if (!organizerdataGridView.Rows[agentrowindex].IsNewRow)
					{
						organizerdataGridView.Rows.RemoveAt(agentrowindex);
						RazorEnhanced.Organizer.CopyTable();
					}
					break;
				case "vendorbuydataGridView":
					if (!vendorbuydataGridView.Rows[agentrowindex].IsNewRow)
					{
						vendorbuydataGridView.Rows.RemoveAt(agentrowindex);
						RazorEnhanced.BuyAgent.CopyTable();
					}
					break;
				case "vendorsellGridView":
					if (!vendorsellGridView.Rows[agentrowindex].IsNewRow)
					{
						vendorsellGridView.Rows.RemoveAt(agentrowindex);
						RazorEnhanced.SellAgent.CopyTable();
					}
					break;
				case "restockdataGridView":
					if (!restockdataGridView.Rows[agentrowindex].IsNewRow)
					{
						restockdataGridView.Rows.RemoveAt(agentrowindex);
						RazorEnhanced.Restock.CopyTable();
					}
					break;
				case "graphfilterdatagrid":
					if (!graphfilterdatagrid.Rows[agentrowindex].IsNewRow)
					{
						graphfilterdatagrid.Rows.RemoveAt(agentrowindex);
						RazorEnhanced.Filters.CopyGraphTable();
					}
					break;
				case "targetbodydataGridView":
					if (!targetbodydataGridView.Rows[agentrowindex].IsNewRow)
						targetbodydataGridView.Rows.RemoveAt(agentrowindex);
					break;
				case "targethueGridView":
					if (!targethueGridView.Rows[agentrowindex].IsNewRow)
						targethueGridView.Rows.RemoveAt(agentrowindex);
					break;
			}

		}

		// ----------------- END AGENT GESTIONE MENU TENDINA -------------------

		// ----------------- START AGENT GESTIONE DRAG DROP -------------------
		private Rectangle dragBoxFromMouseDown;
		private int rowIndexFromMouseDown;
		private int rowIndexOfItemUnderMouseToDrop;

		private void GridView_MouseMove(object sender, MouseEventArgs e)
		{
			if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
			{
				DataGridView grid = (DataGridView)sender;
				if (dragBoxFromMouseDown != Rectangle.Empty &&
					!dragBoxFromMouseDown.Contains(e.X, e.Y))
				{
					DragDropEffects dropEffect = grid.DoDragDrop(
					grid.Rows[rowIndexFromMouseDown],
					DragDropEffects.Move);
				}
			}
		}

		private void GridView_MouseDown(object sender, MouseEventArgs e)
		{
			DataGridView grid = (DataGridView)sender;
			rowIndexFromMouseDown = grid.HitTest(e.X, e.Y).RowIndex;
			if (rowIndexFromMouseDown != -1)
			{
				Size dragSize = SystemInformation.DragSize;
				dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2),
															   e.Y - (dragSize.Height / 2)),
									dragSize);
			}
			else
				dragBoxFromMouseDown = Rectangle.Empty;
		}

		private void GridView_DragOver(object sender, DragEventArgs e)
		{
			e.Effect = DragDropEffects.Move;
		}

		private void GridView_DragDrop(object sender, DragEventArgs e)
		{
			DataGridView grid = (DataGridView)sender;
			Point clientPoint = grid.PointToClient(new Point(e.X, e.Y));

			rowIndexOfItemUnderMouseToDrop = grid.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

			if (rowIndexOfItemUnderMouseToDrop == -1)
				return;

			if (e.Effect == DragDropEffects.Move)
			{
				DataGridViewRow rowToMove = e.Data.GetData(typeof(DataGridViewRow)) as DataGridViewRow;

				if (rowIndexOfItemUnderMouseToDrop >= (grid.RowCount - 1)) // Blocca il drag fuori dalle celle salvate
					return;

				if (rowIndexFromMouseDown >= (grid.RowCount - 1)) // Blocca il drag di una cella non salvata
					return;

				grid.Rows.RemoveAt(rowIndexFromMouseDown);
				grid.Rows.Insert(rowIndexOfItemUnderMouseToDrop, rowToMove);
				switch (grid.Name)
				{
					case "autolootdataGridView":
						AutoLoot.CopyTable();
						break;

					case "scavengerdataGridView":
						Scavenger.CopyTable();
						break;

					case "organizerdataGridView":
						Organizer.CopyTable();
						break;

					case "vendorbuydataGridView":
						BuyAgent.CopyTable();
						break;

					case "vendorsellGridView":
						SellAgent.CopyTable();
						break;

					case "restockdataGridView":
						Restock.CopyTable();
						break;

					case "graphfilterdatagrid":
						RazorEnhanced.Filters.CopyGraphTable();
						break;
				}
			}

		}
		// ----------------- END AGENT GESTIONE DRAG DROP -------------------

		// ----------------- START AGENT EVENTI COMUNI DATAGRID -------------------

		private void GridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
		{
			DataGridView grid = (DataGridView)sender;

			if (!grid.Focused)
				return;

			if (grid.IsCurrentCellDirty)
				grid.CommitEdit(DataGridViewDataErrorContexts.Commit);
		}

		private void GridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
		{
			e.ThrowException = false;
			e.Cancel = false;
		}

		private void GridView_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				if (e.RowIndex != -1)
				{
					DataGridView grid = (DataGridView)sender;

					grid.Rows[e.RowIndex].Selected = true;
					agentrowindex = e.RowIndex;
					agenttype = grid.Name;
					if (grid.Rows[e.RowIndex].Cells.Count > 1)
						grid.CurrentCell = grid.Rows[e.RowIndex].Cells[1];
					else
						grid.CurrentCell = grid.Rows[e.RowIndex].Cells[0];
					datagridMenuStrip.Show(Cursor.Position);
				}
			}

		}

		private void GridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			DataGridView grid = (DataGridView)sender;
			if (!grid.Focused)
				return;

			if (e.ColumnIndex == 0) // Checkbox cambiate di stato genera save
			{
				switch (grid.Name)
				{
					case "autolootdataGridView":
						AutoLoot.CopyTable();
						break;

					case "scavengerdataGridView":
						Scavenger.CopyTable();
						break;

					case "organizerdataGridView":
						Organizer.CopyTable();
						break;

					case "vendorbuydataGridView":
						BuyAgent.CopyTable();
						break;

					case "vendorsellGridView":
						SellAgent.CopyTable();
						break;

					case "restockdataGridView":
						Restock.CopyTable();
						break;

					case "graphfilterdatagrid":
						RazorEnhanced.Filters.CopyGraphTable();
						break;
				}
			}
		}
		// ----------------- END AGENT EVENTI COMUNI DATAGRID -------------------
	}
}

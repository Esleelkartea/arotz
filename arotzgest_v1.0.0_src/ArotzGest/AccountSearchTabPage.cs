/*  ArotzGest
 *  (c) 2006 Webalianza T.I S.L.
 *  Licenciado bajo la GNU General Public License
 *  
 *  Programaci�n: Jorge Moreiro
 *                Iker Est�banez
 * 
 *  Este fichero forma parte de ArotzGest
 *
 * ArotzGest es software libre; puede redistribuirlo y/o modificarlo
 * bajo los t�rminos de la GNU General Public License, tal y como se haya
 * publicada por la Free Software Foundation, en la versi�n 2 de la licencia o
 * (seg�n su elecci�n) cualquier versi�n posterior.
 * 
 * ArotzGest es redistribuido con la intenci�n que sea �til, pero SIN NINGUNA
 * GARANT�A, ni tan solo las garant�as impl�citas de MERCANTABILIDA o ADECUACI�N 
 * A UN DETERMINADO MOTIVO. Lea la GNU General Public License para m�s detalles.
 * 
 * Deber�a haber recibido una copia de la GNU General Public License acompa�ando a 
 * ArotzGest.
 * 
 * �STE PROYECTO HA SIDO SUBVENCIONADO POR SPRI S.A. DENTRO DEL MARCO DEL PROGRAMA
 * KZ LANKIDETZA - m�s informaci�n en http://www.spri.es
 * 
 *
 * */

using System;
using System.Drawing;
using System.Windows.Forms;

class AccountSearchTabPage : TabPage {
  Button acceptButton, cancelButton, createButton, detailButton, searchButton;
  ListView itemListView;
  TextBox codeTextBox, nameTextBox;
  public AccountSearchTabPage () {
    Name = "accountSearch";
    Text = "Cuentas";
    acceptButton = Interface.Button_Create (acceptButton_Click);
    Interface.HeaderPanel_Create (this, 0, 8, "Account", Text, "Buscar");
    Interface.Label_Create (this, 0, 48, "C�digo");
    codeTextBox = Interface.TextBox_Create (this, 128, 48, 54, 1, 9);
    Interface.Label_Create (this, 0, 76, "Nombre");
    nameTextBox = Interface.TextBox_Create (this, 128, 76, 128, 1, 255);
    searchButton = Interface.Button_Create (this, 0, 104, "Buscar", searchButton_Click);
    itemListView = Interface.ListView_Create (this, 0, 144, 0, 0, itemListView_SelectedIndexChanged, itemListView_DoubleClick);
    Interface.ListView_AddColumnHeader (itemListView, 54, "C�digo");
    Interface.ListView_AddColumnHeader (itemListView, 192, "Nombre");
    createButton = Interface.Button_Create (this, 0, 0, "Crear", createButton_Click);
    detailButton = Interface.Button_Create (this, 88, 0, "Detalle", detailButton_Click);
    detailButton.Enabled = false;
    cancelButton = Interface.Button_Create (this, 0, 0, "Cerrar", cancelButton_Click);
  }
  protected override void OnGotFocus (EventArgs e) {
    base.OnGotFocus (e);
    (TopLevelControl as MainForm).DefaultButtons_Set (acceptButton, cancelButton);
    (itemListView.Items.Count == 0 ? codeTextBox as Control : itemListView).Focus ();
  }
  protected override void OnResize (EventArgs e) {
    base.OnResize (e);
    itemListView.Size = new Size (Width, Height - itemListView.Top - 32);
    createButton.Top = Height - 24;
    detailButton.Top = Height - 24;
    cancelButton.Location = new Point (Width - 80, Height - 24);
  }
  void acceptButton_Click (object o, EventArgs e) {
    (!itemListView.ContainsFocus ? searchButton : detailButton).PerformClick ();
  }
  void cancelButton_Click (object o, EventArgs e) {
    Parent.Controls.Remove (this);
  }
  void createButton_Click (object o, EventArgs e) {
    (TopLevelControl as MainForm).AccountDetailTabPage_Open (0);
  }
  void detailButton_Click (object o, EventArgs e) {
    (TopLevelControl as MainForm).AccountDetailTabPage_Open ((int) itemListView.SelectedItems [0].Tag);
  }
  void itemListView_DoubleClick (object o, EventArgs e) {
    detailButton.PerformClick ();
  }
  void itemListView_SelectedIndexChanged (object o, EventArgs e) {
    detailButton.Enabled = itemListView.SelectedItems.Count > 0;
  }
  void searchButton_Click (object o, EventArgs e) {
    Interface.ListView_Clear (itemListView);
    foreach (Database.Account item in Database.Account.Search (codeTextBox.Text, nameTextBox.Text)) Interface.ListView_AddListViewItem (itemListView, new string [] { item.Code.ToString (), item.Name }, item.Id);
    if (itemListView.Items.Count > 0) itemListView.Focus ();
  }
}


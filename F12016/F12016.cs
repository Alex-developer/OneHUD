using System;
using System.Linq;
using System.Windows.Forms;

using OneHUDInterface;

namespace F12016
{
    class F12016 : GameBase, IGame
    {

        #region Constructor
        public F12016()
            : base()
        {
            _name = "f12016";
            _displayName = "F1 2016";
            _author = "Alex Greenland";
            _processNames.Add("F1_2016");
            _connectionType = ConnectionType.BOTH;
        }
        #endregion

        #region Options Dialog
        public override void ShowOptions()
        {
            Options optionsDialog = new Options();

            optionsDialog.ShowDialog();
        }
        #endregion

    }
}

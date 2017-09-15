using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveProject {
    class MenuColoursTable : System.Windows.Forms.ProfessionalColorTable {

        public override Color MenuItemSelected {
            get {
                return Color.FromArgb(255, 45, 45, 45);
            }
        }

        public override Color MenuItemSelectedGradientBegin {
            get {
                return Color.FromArgb(255, 45, 45, 45);
            }
        }

        public override Color MenuItemSelectedGradientEnd {
            get {
                return Color.FromArgb(255, 45, 45, 45);
            }
        }

        public override Color MenuItemBorder {
            get {
                return Color.LightGreen;
            }
        }

        public override Color ButtonSelectedBorder {
            get {
                return Color.LightGreen;
            }
        }

        public override Color ToolStripDropDownBackground {
            get {
                return Color.FromArgb(255, 45, 45, 45);
            }
        }

        public override Color MenuItemPressedGradientBegin {
            get {
                return Color.FromArgb(255, 45, 45, 45);
            }
        }

        public override Color MenuItemPressedGradientEnd {
            get {
                return Color.FromArgb(255, 45, 45, 45);
            }
        }

        public override Color StatusStripGradientBegin {
            get {
                return Color.FromArgb(255, 45, 45, 45);
            }
        }

        public override Color ImageMarginGradientBegin {
            get {
                return Color.Transparent;
            }
        }
        public override Color ImageMarginGradientMiddle {
            get {
                return Color.Transparent;
            }
        }
        public override Color ImageMarginGradientEnd {
            get {
                return Color.Transparent;
            }
        }
    }
}
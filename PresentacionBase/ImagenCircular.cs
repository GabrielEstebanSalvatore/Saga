using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PresentacionBase
{
    public class ImagenCircular : PictureBox
    {
        protected override void OnPaint(PaintEventArgs pe)
        {
            GraphicsPath g = new GraphicsPath();
            g.AddEllipse(0, 0, ClientSize.Width, ClientSize.Height);
            this.Region = new System.Drawing.Region(g);
            base.OnPaint(pe);   
        }
    }
}

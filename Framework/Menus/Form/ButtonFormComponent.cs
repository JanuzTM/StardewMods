﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;

namespace Entoarox.Framework.Menus
{
    public class ButtonFormComponent : BaseFormComponent
    {
        protected static Rectangle ButtonNormal = new Rectangle(256, 256, 10, 10);
        protected static Rectangle ButtonHover = new Rectangle(267, 256, 10, 10);
        public event ButtonPressed Handler;
        protected string Label;
        protected int LabelOffset;
        protected bool Hovered = false;
        public ButtonFormComponent(Point position, string label, ButtonPressed handler=null) : this(position,50,label,handler)
        {

        }
        public ButtonFormComponent(Point position, int width, string label, ButtonPressed handler=null)
        {
            int labelWidth = GetStringWidth(label, Game1.smallFont);
            width = Math.Max(width,labelWidth+4);
            LabelOffset = (int)Math.Round((width - labelWidth) / 2D);
            SetScaledArea(new Rectangle(position.X, position.Y, width, 10));
            Label = label;
            if (handler!=null)
                Handler += handler;
        }
        public override void LeftClick(Point p, Point o, IComponentCollection c, FrameworkMenu m)
        {
            if (Disabled)
                return;
            Handler?.Invoke(this, c, m);
        }
        public override void HoverIn(Point p, Point o, IComponentCollection c, FrameworkMenu m)
        {
            if (Disabled)
                return;
            Hovered = true;
        }
        public override void HoverOut(Point p, Point o, IComponentCollection c, FrameworkMenu m)
        {
            if (Disabled)
                return;
            Hovered = false;
        }
        public override void Draw(SpriteBatch b, Point o)
        {
            Rectangle r = Hovered ? ButtonHover : ButtonNormal;
            // Begin
            b.Draw(Game1.mouseCursors, new Rectangle(Area.X + o.X, Area.Y + o.Y, 2 * Game1.pixelZoom, Area.Height), new Rectangle(r.X, r.Y, 2, r.Height), Color.White, 0, Vector2.Zero, SpriteEffects.None, 1f);
            // End
            b.Draw(Game1.mouseCursors, new Rectangle(Area.X + o.X + Area.Width-2*Game1.pixelZoom, Area.Y + o.Y, 2 * Game1.pixelZoom, Area.Height), new Rectangle(r.X+r.Width-2, r.Y, 2, r.Height), Color.White, 0, Vector2.Zero, SpriteEffects.None, 1f);
            // Center
            b.Draw(Game1.mouseCursors, new Rectangle(Area.X + o.X + 2 * Game1.pixelZoom, Area.Y + o.Y, Area.Width - 4 * Game1.pixelZoom, Area.Height), new Rectangle(r.X+2, r.Y, r.Width - 4, r.Height), Color.White, 0, Vector2.Zero, SpriteEffects.None, 1f);
            // Text
            Utility.drawTextWithShadow(b, Label, Game1.smallFont, new Vector2(o.X + Area.X + LabelOffset*Game1.pixelZoom, o.Y + Area.Y + Game1.pixelZoom*2), Game1.textColor * (Disabled?0.7f:1));
        }
    }
}
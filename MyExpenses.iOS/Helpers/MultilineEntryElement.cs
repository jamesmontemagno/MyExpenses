using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using MonoTouch.Dialog;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace MyExpenses.iOS.Helpers
{
  public class MultilineEntryElement : Element, IElementSizing
  {
    public string Value;
    static NSString ekey = new NSString("EntryElement");
    bool isPassword;
    UITextView entry;
    string placeholder;
    static UIFont font = UIFont.BoldSystemFontOfSize(17);

    /// <summary>
    /// Constructs an EntryElement with the given caption, placeholder and initial value.
    /// </summary>
    /// <param name="caption">
    /// The caption to use
    /// </param>
    /// <param name="placeholder">
    /// Placeholder to display.
    /// </param>
    /// <param name="value">
    /// Initial value.
    /// </param>
    public MultilineEntryElement(string caption, string placeholder, string value)
      : base(caption)
    {
      Value = value;
      this.placeholder = placeholder;
    }

    /// <summary>
    /// Constructs  an EntryElement for password entry with the given caption, placeholder and initial value.
    /// </summary>
    /// <param name="caption">
    /// The caption to use
    /// </param>
    /// <param name="placeholder">
    /// Placeholder to display.
    /// </param>
    /// <param name="value">
    /// Initial value.
    /// </param>
    /// <param name="isPassword">
    /// True if this should be used to enter a password.
    /// </param>

    public override string Summary()
    {
      return Value;
    }

    // 
    // Computes the X position for the entry by aligning all the entries in the Section
    //
    SizeF ComputeEntryPosition(UITableView tv, UITableViewCell cell)
    {
      Section s = Parent as Section;
      if (s.EntryAlignment.Width != 0)
        return s.EntryAlignment;

      SizeF max = new SizeF(-1, -1);
      foreach (var e in s.Elements)
      {
        var ee = e as MultilineEntryElement;
        if (ee == null)
          continue;

        var size = tv.StringSize(ee.Caption, font);
        if (size.Width > max.Width)
          max = size;
      }
      s.EntryAlignment = new SizeF(25 + Math.Min(max.Width, 160), max.Height);
      return s.EntryAlignment;
    }

    public override UITableViewCell GetCell(UITableView tv)
    {
      var cell = tv.DequeueReusableCell(ekey);
      if (cell == null)
      {
        cell = new UITableViewCell(UITableViewCellStyle.Default, ekey);
        cell.SelectionStyle = UITableViewCellSelectionStyle.None;
      }
      else
        RemoveTag(cell, 1);


      if (entry == null)
      {
        SizeF size = ComputeEntryPosition(tv, cell);
        /*entry = new UITextField (new RectangleF (size.Width, (cell.ContentView.Bounds.Height-size.Height)/2-1, 320-size.Width, size.Height)){
          Tag = 1,
          Placeholder = placeholder,
          SecureTextEntry = isPassword
        };*/
        entry = new UITextView(new RectangleF(size.Width, (cell.ContentView.Bounds.Height - size.Height) / 2 - 1, 320 - size.Width, 96));
        entry.Text = Value ?? "";
        entry.AutoresizingMask = UIViewAutoresizing.FlexibleWidth |
          UIViewAutoresizing.FlexibleLeftMargin;

        entry.Ended += delegate
        {
          Value = entry.Text;
        };
        entry.ReturnKeyType = UIReturnKeyType.Done;
        entry.Changed += delegate(object sender, EventArgs e)
        {
          int i = entry.Text.IndexOf("\n", entry.Text.Length - 1);
          if (i > -1)
          {
            entry.Text = entry.Text.Substring(0, entry.Text.Length - 1);
            entry.ResignFirstResponder();
          }
        };
      }


      cell.TextLabel.Text = Caption;
      cell.ContentView.AddSubview(entry);
      return cell;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        entry.Dispose();
        entry = null;
      }
    }

    public float GetHeight(UITableView tableView, NSIndexPath indexPath)
    {
      return 112;
    }
  }
}
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;
using jellybins.Views;
using Wpf.Ui.Controls;
using ListView = Wpf.Ui.Controls.ListView;
using TextBlock = System.Windows.Controls.TextBlock;

namespace jellybins.Middleware;
/*
 * Jelly Bins (C) Толстопятов Алексей 2024
 *         Item Creator
 * Класс, предоставляющий логику заполнения элементов окна
 */
public static class PageFiller
{
    public static void SetFlagDescription(
        string content,
        SolidColorBrush foreground,
        ref BinaryHeaderPage bin
    )
    {
        bin.FlagsNames.Items.Add(
            new TextBlock()
            {
                Foreground = foreground,
                Text = content,
            });
    }
    public static void SetFlag(ref ListView item, string content, SolidColorBrush color) =>
        item.Items.Add(new TextBlock()
        {
            Foreground = color,
            Text = content,
        });

    public static void SetCaption(ref Label label, string content) =>
        label.Content = content;

    public static void SetFlags(Dictionary<string, string[]> flags, ref BinaryHeaderPage hPage)
    {
        int brushCounter = 0;
        SolidColorBrush[] brushes = 
        {
            Brushes.Goldenrod,
            Brushes.Yellow,
            Brushes.LimeGreen,
            Brushes.Cyan,
            Brushes.CornflowerBlue,
            Brushes.MediumPurple
        };
        
        foreach (var flag in flags)
        {
            hPage.FlagsNames.Items.Add(
                
                new TextBlock()
                {
                    Foreground = brushes[brushCounter],
                    Text = flag.Key
                });
            foreach (var section in flag.Value)
            {
                hPage.FlagsView.Items.Add(
                    new Card()
                    {
                        Content =
                            new TextBlock()
                            {
                                Foreground = brushes[brushCounter],
                                Text = section
                            },
                        Width = 300
                    });
            }
            ++brushCounter;
        }

        hPage.FlagsView.SpacingMode = SpacingMode.StartAndEndOnly;
    }
}
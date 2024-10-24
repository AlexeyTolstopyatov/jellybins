using System.Windows.Controls;
using System.Windows.Media;
using jellybins.Views;
using ListView = Wpf.Ui.Controls.ListView;
using TextBlock = System.Windows.Controls.TextBlock;

namespace jellybins.Middleware;
/*
 * Jelly Bins (C) Толстопятов Алексей 2024
 *         Item Creator
 * Класс, предоставляющий логику заполнения элементов окна
 */
public static class JbElement
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
}
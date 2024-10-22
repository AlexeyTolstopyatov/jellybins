using System.Windows.Controls;
using System.Windows.Media;
using ListView = Wpf.Ui.Controls.ListView;
using TextBlock = System.Windows.Controls.TextBlock;

namespace jellybins.Middleware;
/*
 * Jelly Bins (C) Толстопятов Алексей 2024
 *         Item Creator
 * Класс, предоставляющий логику заполнения элементов окна
 */
public class ItemCreator
{
    public static void NewListItem(ref ListView item, string content, SolidColorBrush color) =>
        item.Items.Add(new TextBlock()
        {
            Foreground = color,
            Text = content,
        });

    public static void NewText(ref Label label, string content) =>
        label.Content = content;
}
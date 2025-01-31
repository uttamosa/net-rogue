using rogue;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace EnemyEditor
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Console.WriteLine(sender.ToString());
        }
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void AddEnemyToList(object sender, RoutedEventArgs e)
        {
            string name = input1.Text;
            string hpString = input3.Text;
            string spriteid = input2.Text;

            int hpInt;
            int spriteint;
            if (Int32.TryParse(hpString, out hpInt) && Int32.TryParse(spriteid, out spriteint))
            {
                if (string.IsNullOrEmpty(name) == false)
                {
                    itemlist.Items.Add(new rogue.enemy(name, hpInt, spriteint));
                }
            }
        }
        private void SaveEnemiesToJSON(object sender, RoutedEventArgs e)
        {
            int EnemyCount = itemlist.Items.Count;

            List<rogue.enemy> tempList = new List<rogue.enemy>();

            // Käy jokainen vihollinen läpi ja hae sen tiedot.
            for (int i = 0; i < EnemyCount; i++)
            {
                // Muuta ListBox elementissä oleva Object tyyppinen olio Enemy tyyppiseksi
                rogue.enemy enemy = (rogue.enemy)itemlist.Items[i];
                // Lisää vihollinen listaan
                tempList.Add(enemy);
            }

            string enemiesArrayJSON = Newtonsoft.Json.JsonConvert.SerializeObject(tempList);
            
            string filename = "..\\..\\..\\..\\rogue\\Enemies.json";
            using (StreamWriter enemyWriter = new StreamWriter(filename))
            {
                enemyWriter.Write(enemiesArrayJSON);
            }

            errorlabel.Content = "Write OK!";
        }
    }
}

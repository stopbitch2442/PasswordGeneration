using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordGeneration
{
    public class RandomizeThreeWordForPassword
    {
        private readonly List<string> Passwords = new List<string>
        {
    "c8DzJeTC9t",
    "rvbrYASYyh",
        };

        private readonly Random random = new Random();

        private void button2_Click(object sender, EventArgs e)
        {
            var index = random.Next(Passwords.Count);
            var password = Passwords[index];
            textBox1.Text = password;
        }
    }
}

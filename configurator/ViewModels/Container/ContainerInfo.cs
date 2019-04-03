using System.Drawing;

namespace Configurator
{
    public class ContainerInfo
    {
        public string Name { get; set; }
        public Rectangle Area { get; set; }

        public ContainerInfo(string name, Rectangle area)
        {
            Name = name;
            Area = area;
        }
    }
}

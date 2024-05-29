using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    public enum FigureColor
    {
        White,
        Black
    }
    public enum FigureType
    {
        Checker,
        King
    }
    internal class Figure
    {
        public Figure(FigureColor fc)
        {
            figureColor = fc;
        }
       
        FigureType figureType { get; set; }
        FigureColor figureColor { get; set; }
    }
}

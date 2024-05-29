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
        public Figure(FigureColor fc, FigureType ft)
        {
            figureColor = fc;
            figureType = ft;
        }

        FigureType figureType;
        FigureColor figureColor;
    }
}

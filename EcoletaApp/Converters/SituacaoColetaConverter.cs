using Microsoft.Maui.Graphics.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecoleta.Converters
{
    public class SituacaoColetaConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
           ColorTypeConverter converter = new ColorTypeConverter();

           string situacaoColeta = (string)value;
            if (situacaoColeta == "Completa" || situacaoColeta == "Completo" || situacaoColeta == "Completada" || situacaoColeta == "completo" || situacaoColeta == "completa" || situacaoColeta == "completada" || situacaoColeta == "concluida" || situacaoColeta == "Concluida")
                return (Color)converter.ConvertFromInvariantString("YellowGreen");
            else if (situacaoColeta == "Pendente" || situacaoColeta == "Analisando" || situacaoColeta == "pendente" || situacaoColeta == "analisando" )
                return (Color)converter.ConvertFromInvariantString("OrangeRed");
            else if (situacaoColeta == "Recusada" || situacaoColeta == "Invalida" || situacaoColeta == "invalida" || situacaoColeta == "recusada")
                return (Color)converter.ConvertFromInvariantString("Red");
            else
            return (Color)converter.ConvertFromInvariantString("Yellow");
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

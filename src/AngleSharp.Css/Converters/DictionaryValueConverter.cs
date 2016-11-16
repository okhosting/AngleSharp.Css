﻿namespace AngleSharp.Css.Converters
{
    using AngleSharp.Css.Dom;
    using AngleSharp.Css.Parser;
    using AngleSharp.Text;
    using System;
    using System.Collections.Generic;
    using System.IO;

    sealed class DictionaryValueConverter<T> : IValueConverter
    {
        private readonly IDictionary<String, T> _values;

        public DictionaryValueConverter(IDictionary<String, T> values)
        {
            _values = values;
        }

        public ICssValue Convert(StringSource source)
        {
            var ident = source.ParseIdent();
            var mode = default(T);
            return ident != null && _values.TryGetValue(ident, out mode) ?
                new EnumeratedValue(_values, mode) : null;
        }

        private sealed class EnumeratedValue : ICssValue
        {
            private readonly IDictionary<String, T> _values;
            private readonly T _data;

            public EnumeratedValue(IDictionary<String, T> values, T data)
            {
                _values = values;
                _data = data;
            }

            public String CssText
            {
                get
                {
                    foreach (var value in _values)
                    {
                        if (value.Value.Equals(_data))
                        {
                            return value.Key;
                        }
                    }

                    return String.Empty;
                }
            }

            public void ToCss(TextWriter writer, IStyleFormatter formatter)
            {
                writer.Write(CssText);
            }
        }
    }
}
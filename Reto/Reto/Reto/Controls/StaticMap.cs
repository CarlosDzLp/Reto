using Reto.Helpers;
using Reto.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Reto.Controls
{
    public class StaticMap : Image
    {
        #region Variables

        /// <summary>
        /// Bindable property for the <see cref="Pins"/> property
        /// </summary>
        public static readonly BindableProperty PinsProperty = BindableProperty.Create(nameof(Pins), typeof(IEnumerable<PinModel>), typeof(StaticMap), propertyChanged: OnPropertyChanged);

        /// <summary>
        /// Bindable property for the <see cref="Polylines"/> property
        /// </summary>
        public static readonly BindableProperty PolylinesProperty = BindableProperty.Create(nameof(Polylines), typeof(IEnumerable<string>), typeof(StaticMap), null, propertyChanged: OnPropertyChanged);

        /// <summary>
        /// Bindable property for the <see cref="Zoom"/> property
        /// </summary>
        public static readonly BindableProperty ZoomProperty = BindableProperty.Create(nameof(Zoom), typeof(int?), typeof(StaticMap), null, propertyChanged: OnPropertyChanged);

        /// <summary>
        /// Bindable property for the <see cref="CachingEnabled"/> property
        /// </summary>
        public static readonly BindableProperty CachingEnabledProperty = BindableProperty.Create(nameof(CachingEnabled), typeof(bool), typeof(StaticMap), false, propertyChanged: OnPropertyChanged);

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public StaticMap()
        { }

        #endregion

        #region Properties

        /// <summary>
        /// Pins
        /// </summary>
        public IEnumerable<PinModel> Pins
        {
            get => (IEnumerable<PinModel>)GetValue(PinsProperty);
            set => SetValue(PinsProperty, value);
        }

        /// <summary>
        /// Polylines
        /// </summary>
        public IEnumerable<string> Polylines
        {
            get => (IEnumerable<string>)GetValue(PolylinesProperty);
            set => SetValue(PolylinesProperty, value);
        }

        /// <summary>
        /// ApiKey
        /// </summary>
        public string ApiKey { get; set; } = GoogleMapsKey.KeyDroid;

        /// <summary>
        /// Zoom level
        /// </summary>
        public int? Zoom
        {
            get => (int?)GetValue(ZoomProperty);
            set => SetValue(ZoomProperty, value);
        }

        /// <summary>
        /// Is caching enabled
        /// </summary>
        public bool CachingEnabled
        {
            get => (bool)GetValue(CachingEnabledProperty);
            set => SetValue(CachingEnabledProperty, value);
        }

        #endregion

        #region Protected

        /// <summary>
        /// New size allocated, we need to update the camera on the map (if applicable)
        /// </summary>
        /// <param name="width">width</param>
        /// <param name="height">height</param>
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            SetImageSource();
        }

        #endregion

        #region Private

        /// <summary>
        /// Handle the change a property
        /// </summary>
        /// <param name="bindable">Bindable object</param>
        /// <param name="o">Old values</param>
        /// <param name="n">New values</param>
        private static void OnPropertyChanged(BindableObject bindable, object o, object n)
        {
            if (!(bindable is StaticMap map))
                return;
            map.SetImageSource();
        }

        /// <summary>
        /// Set the image source
        /// </summary>
        private void SetImageSource()
        {

            if (Width < 1 || Height < 1)
                return;
            string url = $"https://maps.googleapis.com/maps/api/staticmap?size={Convert.ToInt32(Width)}x{Convert.ToInt32(Height)}&scale=2&format=png&maptype=roadmap&style=feature:administrative%7Celement:geometry.fill%7Ccolor:0xd6e2e6&style=feature:administrative%7Celement:geometry.stroke%7Ccolor:0xcfd4d5&style=feature:administrative%7Celement:labels.text.fill%7Ccolor:0x7492a8&style=feature:administrative.neighborhood%7Celement:labels.text.fill%7Clightness:25&style=feature:landscape.man_made%7Celement:geometry.fill%7Ccolor:0xdde2e3&style=feature:landscape.man_made%7Celement:geometry.stroke%7Ccolor:0xcfd4d5&style=feature:landscape.natural%7Celement:geometry.fill%7Ccolor:0xdde2e3&style=feature:landscape.natural%7Celement:labels.text.fill%7Ccolor:0x7492a8&style=feature:landscape.natural.terrain%7Cvisibility:off&style=feature:poi%7Celement:geometry.fill%7Ccolor:0xdde2e3&style=feature:poi%7Celement:labels.icon%7Csaturation:-100&style=feature:poi%7Celement:labels.text.fill%7Ccolor:0x588ca4&style=feature:poi.park%7Celement:geometry.fill%7Ccolor:0xa9de83&style=feature:poi.park%7Celement:geometry.stroke%7Ccolor:0xbae6a1&style=feature:poi.sports_complex%7Celement:geometry.fill%7Ccolor:0xc6e8b3&style=feature:poi.sports_complex%7Celement:geometry.stroke%7Ccolor:0xbae6a1&style=feature:road%7Celement:labels.icon%7Csaturation:-45%7Clightness:10%7Cvisibility:on&style=feature:road%7Celement:labels.text.fill%7Ccolor:0x41626b&style=feature:road.arterial%7Celement:geometry.fill%7Ccolor:0xffffff&style=feature:road.highway%7Celement:geometry.fill%7Ccolor:0xc1d1d6&style=feature:road.highway%7Celement:geometry.stroke%7Ccolor:0xa6b5bb&style=feature:road.highway%7Celement:labels.icon%7Cvisibility:on&style=feature:road.highway.controlled_access%7Celement:geometry.fill%7Ccolor:0x9fb6bd&style=feature:road.local%7Celement:geometry.fill%7Ccolor:0xffffff&style=feature:transit%7Celement:labels.icon%7Csaturation:-70&style=feature:transit.line%7Celement:geometry.fill%7Ccolor:0xb4cbd4&style=feature:transit.line%7Celement:labels.text.fill%7Ccolor:0x588ca4&style=feature:transit.station%7Celement:labels.text.fill%7Ccolor:0x008cb5&style=feature:transit.station.airport%7Celement:geometry.fill%7Csaturation:-100%7Clightness:-5&style=feature:water%7Celement:geometry.fill%7Ccolor:0xa6cbe3";
            if (Zoom.HasValue)
                url += $"&zoom={Zoom}";
            url += $"&language={CultureInfo.CurrentCulture.TwoLetterISOLanguageName}";
            if (Pins != null)
            {
                foreach (var pin in Pins)
                {
                    url += $"&markers=color:0xDA274D|{pin.Latitude},{pin.Longitude}";
                }
            }
            if (Polylines != null)
            {
                foreach (var polyline in Polylines)
                {
                    url += $"&path=color:0xDA274D|enc:{polyline}";
                }
            }
            url += $"&key={ApiKey}";
            Source = new UriImageSource
            {
                Uri = new Uri(url),
                CachingEnabled = CachingEnabled
            };
        }

        #endregion
    }
}

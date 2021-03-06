#if PRE_4
#define PRE_5
#endif
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace Hurst.BaseLibWpf.Display
{
    #region class MessageBoxViewModel
    /// <summary>
    /// A minimalist view-model-viewmodel for the MessageBoxWindow view.
    /// </summary>
    public class MessageBoxViewModel : ViewModel
    {
        #region constructor

        public MessageBoxViewModel()
        {
            //CBL  But how to get logging when Application.Current does NOT yield my App, as within Cider?
            //var logManager = new NLogManagerWrapper();
            //logManager.GetLogger("MessageBoxLogger").LogInfo("enter MessageBoxViewModel.ctor, IsInDesignMode=" + this.IsInDesignMode);
#if DEBUG
            var loggingProvider = Application.Current as ISimpleLoggingProvider;
            if (loggingProvider != null)
            {
                //loggingProvider.LogManager.GetLogger("MessageBoxLogger").LogInfo("enter MessageBoxViewModel.ctor, IsInDesignMode=" + this.IsInDesignMode);
                loggingProvider.LogInfo("enter MessageBoxViewModel.ctor, IsInDesignMode=" + this.IsInDesignMode);
            }
#endif

            if (this.IsInDesignMode)
            {
                _configuration = new MessageBoxConfiguration(MessageBoxType.Error);
                Configuration.ButtonFlags =
                      MessageBoxButtons.Yes
                    | MessageBoxButtons.No
                    | MessageBoxButtons.Ok
                    | MessageBoxButtons.Cancel
                    | MessageBoxButtons.Close
                    | MessageBoxButtons.Ignore
                    | MessageBoxButtons.Retry;
                Title = "Vendor Product: Caption";
                //string summaryTextPattern = "The Summary Text.";
                //SummaryText = summaryTextPattern.ExpandTo(20);
                SummaryText = "This is the SummaryText right here. Right here.";
                string detailTextPattern = "The Details Texts. X";
                DetailText = detailTextPattern.ExpandTo(63);
                //DetailText = "The Details Texts. XThe Details Texts. XThe Details Texts._XThe Details Texts._XThe Details Texts._XThe";
                //DetailText = "The Details Text";
                //Configuration.BackgroundTexture = MessageBoxBackgroundTexturePreset.BlueTexture;
                DefaultConfiguration.IsUsingNewerIcons = false;
                IsCustomElementVisible = true;
                IsGradiantShown = false;
                SummaryTextColor = new SolidColorBrush(Colors.Black);
            }
            IsCustomElementVisible = true;
            IsGradiantShown = false;
            SummaryTextColor = new SolidColorBrush(Colors.Black);
        }

        public MessageBoxViewModel(MessageBox manager, MessageBoxConfiguration options)
        {
            _configuration = options;
            DetailText = options.DetailText;
            SummaryText = options.SummaryText;
            // If there is no caption, then just put the caption-prefix without the spacer.
            if (StringLib.HasNothing(options.CaptionAfterPrefix))
            {
                if (StringLib.HasNothing(options.CaptionPrefix))
                {
                    Title = manager.CaptionPrefix;
                }
                else
                {
                    Title = options.CaptionPrefix;
                }
            }
            else
            {
                if (StringLib.HasNothing(options.CaptionPrefix))
                {
                    Title = manager.CaptionPrefix + ": " + options.CaptionAfterPrefix;
                }
                else
                {
                    Title = options.CaptionPrefix + ": " + options.CaptionAfterPrefix;
                }
            }
            IsCustomElementVisible = true;
            IsGradiantShown = false;
            SummaryTextColor = new SolidColorBrush(Colors.Black);
        }

        public static MessageBoxViewModel GetInstance(MessageBox manager, MessageBoxConfiguration options)
        {
            _theInstance = new MessageBoxViewModel(manager, options);
            return _theInstance;
        }

        public static MessageBoxViewModel Instance
        {
            get
            {
                // This is intended only for design-mode.
                if (_theInstance == null)
                {
                    _theInstance = new MessageBoxViewModel();
                }
                return _theInstance;
            }
        }
        #endregion constructor

        public RelayCommand CopyCommand { get; set; }
        public RelayCommand StayCommand { get; set; }

        #region public properties

        #region BackgroundBrush
        /// <summary>
        /// Get or set the ImageBrush that gives the message-box Window it's background texture, if any. Default is null, indicating no image.
        /// </summary>
        public Brush BackgroundBrush
        {
            get
            {
                Brush backgroundBrush;
                MessageBoxBackgroundTexturePreset texture = MessageBox.DefaultConfiguration.BackgroundTexture;
                ImageBrush imageBrush = null;
                string imageFilename = null;
                Rect rectViewport = new Rect(0, 0, 40, 40);
                switch (texture)
                {
                    case MessageBoxBackgroundTexturePreset.BlueTexture:
                        imageFilename = "bgBlueTexture.png";
                        rectViewport.Width = 593;
                        rectViewport.Height = 463;
                        break;
                    case MessageBoxBackgroundTexturePreset.BlueMarble:
                        imageFilename = "bgBlueMarble.jpg";
                        rectViewport.Width = rectViewport.Height = 192;
                        break;
                    case MessageBoxBackgroundTexturePreset.BrownMarble1:
                        imageFilename = "bgBrownMarble1.gif";
                        rectViewport.Width = 300;
                        rectViewport.Height = 397;
                        break;
                    case MessageBoxBackgroundTexturePreset.BrownMarble2:
                        imageFilename = "bgBrownMarble2.jpg";
                        rectViewport.Width = rectViewport.Height = 256;
                        break;
                    case MessageBoxBackgroundTexturePreset.BrownTexture1:
                        imageFilename = "bgBrownTexture1.jpg";
                        rectViewport.Width = rectViewport.Height = 89;
                        break;
                    case MessageBoxBackgroundTexturePreset.BrownTexture2:
                        imageFilename = "bgBrownTexture2.gif";
                        rectViewport.Width = 60;
                        rectViewport.Height = 90;
                        break;
                    case MessageBoxBackgroundTexturePreset.BrushedMetal:
                        imageFilename = "bgBrushedMetal.png";
                        rectViewport.Width = 980;
                        rectViewport.Height = 192;
                        break;
                    case MessageBoxBackgroundTexturePreset.GrayMarble:
                        imageFilename = "bgGrayMarble.jpg";
                        rectViewport.Width = rectViewport.Height = 128;
                        break;
                    case MessageBoxBackgroundTexturePreset.GrayTexture1:
                        imageFilename = "TextureR40x40.png";
                        rectViewport.Width = rectViewport.Height = 40;
                        break;
                    case MessageBoxBackgroundTexturePreset.GrayTexture2:
                        imageFilename = "bgGrayTexture2.tif";
                        rectViewport.Width = 429;
                        rectViewport.Height = 440;
                        break;
                    case MessageBoxBackgroundTexturePreset.GreenTexture1:
                        imageFilename = "bgGreenTexture1.gif";
                        rectViewport.Width = 102;
                        rectViewport.Height = 101;
                        break;
                    case MessageBoxBackgroundTexturePreset.GreenTexture2:
                        imageFilename = "bgGreenTexture2.gif";
                        rectViewport.Width = 106;
                        rectViewport.Height = 108;
                        break;
                    case MessageBoxBackgroundTexturePreset.GreenTexture3:
                        imageFilename = "bgGreenTexture3.jpg";
                        rectViewport.Width = rectViewport.Height = 200;
                        break;
                    default:
                        break;
                }
                if (imageFilename != null)
                {
                    imageBrush = new ImageBrush();
                    imageBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Hurst.BaseLibWpf;Component/Images/" + imageFilename));
                    imageBrush.ViewportUnits = BrushMappingMode.Absolute;
                    imageBrush.Stretch = Stretch.None;
                    imageBrush.Viewport = rectViewport;
                    imageBrush.TileMode = TileMode.Tile;
                    backgroundBrush = imageBrush;
                }
                else
                {
                    backgroundBrush = Brushes.White;
                }
                //TODO: This is just tinkeriing..
                if (texture == MessageBoxBackgroundTexturePreset.GreenTexture3)
                {
                    imageBrush = new ImageBrush();
                    imageBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Hurst.BaseLibWpf;Component/Images/" + imageFilename));
                    imageBrush.ViewportUnits = BrushMappingMode.Absolute;
                    imageBrush.Stretch = Stretch.None;
                    imageBrush.Viewport = rectViewport;
                    imageBrush.TileMode = TileMode.Tile;
                    //_summaryTextBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8E9E71"));
                    //_detailTextBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8E9E71"));
                }
                return backgroundBrush;
            }
        }
        #endregion

        #region ButtonMargin
        /// <summary>
        /// Get the value to use for the Margin property of each of the buttons.
        /// </summary>
        public Thickness ButtonMargin
        {
            get
            {
                // Button width by default had been 55, which works for 6 buttons.
                // If we only have a small subset of the available buttons showing, then give them a bit more breathing room..
                int n = GetNumberOfButtons();
                if (n < 4)
                {
                    return new Thickness(9.0, 2.0, 9.0, 2.0);
                }
                else if (n < 5)
                {
                    return new Thickness(4.0, 2.0, 5.0, 2.0);
                }
                else if (n < 6)
                {
                    return new Thickness(3.0, 2.0, 3.0, 2.0);
                }
                else if (n < 7)
                {
                    return new Thickness(2.0, 2.0, 2.0, 2.0);
                }
                else
                {
                    return new Thickness(1.5, 2.0, 1.5, 2.0);
                }
            }
        }
        #endregion

        #region ButtonWidth
        /// <summary>
        /// Get the value to use for the Width property of each of the buttons.
        /// </summary>
        public double ButtonWidth
        {
            get
            {
                // Button width by default had been 55, which works for 6 buttons.
                // If we only have a small subset of the available buttons showing, then give them a bit more breathing room..
                double buttonWidth;
                int n = GetNumberOfButtons();
                if (n < 5)
                {
                    buttonWidth = 80;
                }
                else if (n < 6)
                {
                    buttonWidth = 65;
                }
                else if (n < 7)
                {
                    buttonWidth = 57;
                }
                else
                {
                    buttonWidth = 49;
                }
                return buttonWidth;
            }
        }
        #endregion

        #region ButtonPanelBackground
        /// <summary>
        /// Get the Brush to use for the background fill of the lower panel that contains the buttons. Default is null.
        /// </summary>
        public Brush ButtonPanelBackground
        {
            get
            {
                string sButtonPanelBackground = null;
                Brush buttonPanelBackground;

                switch (this.DefaultConfiguration.BackgroundTexture)
                {
                    case MessageBoxBackgroundTexturePreset.BrownMarble1:
                        sButtonPanelBackground = "#FFF0F0F0";
                        break;
                    case MessageBoxBackgroundTexturePreset.BrownMarble2:
                        sButtonPanelBackground = "#FFF0F0F0";
                        break;
                    case MessageBoxBackgroundTexturePreset.BrownTexture2:
                        sButtonPanelBackground = "#FFF0F0F0";
                        break;
                    case MessageBoxBackgroundTexturePreset.BrushedMetal:
                        sButtonPanelBackground = "#A5A5A5";
                        break;
                    case MessageBoxBackgroundTexturePreset.GrayMarble:
                        sButtonPanelBackground = "#C0BBB8";
                        break;
                    case MessageBoxBackgroundTexturePreset.GrayTexture1:
                        sButtonPanelBackground = "#ABABAB";
                        break;
                    case MessageBoxBackgroundTexturePreset.GrayTexture2:
                        sButtonPanelBackground = "#D5D5D5";
                        break;
                    case MessageBoxBackgroundTexturePreset.GreenTexture1:
                        sButtonPanelBackground = "#B2B08B";
                        break;
                    case MessageBoxBackgroundTexturePreset.GreenTexture2:
                        sButtonPanelBackground = "#999966";
                        break;
                    case MessageBoxBackgroundTexturePreset.GreenTexture3:
                        //sButtonPanelBackground = "#8E9E71";
                        buttonPanelBackground = new SolidColorBrush(Colors.Transparent);
                        return buttonPanelBackground;
                }

#if !SILVERLIGHT
                var bc = new System.Windows.Media.BrushConverter();
#endif
                if (sButtonPanelBackground != null)
                {
                    buttonPanelBackground = (SolidColorBrush)bc.ConvertFromString(sButtonPanelBackground);
                }
                else
                {
                    // or, the default value set in XAML had been #FFF0F0F0
                    //_buttonPanelBackground = Brushes.Gray; //(SolidColorBrush)bc.ConvertFromString("Gray");
                    buttonPanelBackground = (SolidColorBrush)bc.ConvertFromString("#FFF0F0F0");
                    //_buttonPanelBackground = null;
                }
                return buttonPanelBackground;
            }
        }
        #endregion

        #region button text

        public string ButtonAbortText
        {
            get { return _configuration.ButtonAbortText; }
        }

        public string ButtonCancelText
        {
            get { return _configuration.ButtonCancelText; }
        }

        public string ButtonCloseText
        {
            get { return _configuration.ButtonCloseText; }
        }

        public string ButtonIgnoreText
        {
            get { return _configuration.ButtonIgnoreText; }
        }

        public string ButtonNoText
        {
            get { return _configuration.ButtonNoText; }
        }

        public string ButtonOkText
        {
            get { return _configuration.ButtonOkText; }
        }

        public string ButtonRetryText
        {
            get { return _configuration.ButtonRetryText; }
        }

        public string ButtonYesText
        {
            get { return _configuration.ButtonYesText; }
        }

        #endregion button text

        #region button tooltips

        public string ButtonAbortToolTip
        {
            get { return _configuration.ButtonAbortToolTip; }
        }

        public string ButtonCancelToolTip
        {
            get { return _configuration.ButtonCancelToolTip; }
        }

        public string ButtonCloseToolTip
        {
            get { return _configuration.ButtonCloseToolTip; }
        }

        public string ButtonIgnoreToolTip
        {
            get { return _configuration.ButtonIgnoreToolTip; }
        }

        public string ButtonNoToolTip
        {
            get { return _configuration.ButtonNoToolTip; }
        }

        public string ButtonOkToolTip
        {
            get { return _configuration.ButtonOkToolTip; }
        }

        public string ButtonRetryToolTip
        {
            get { return _configuration.ButtonRetryToolTip; }
        }

        public string ButtonYesToolTip
        {
            get { return _configuration.ButtonYesToolTip; }
        }

        #endregion button tooltips

        #region button visibility

        /// <summary>
        /// Get the Visibility property for the Cancel button.
        /// </summary>
        public Visibility ButtonCancelVisibility
        {
            get
            {
                if ((Configuration.ButtonFlags & MessageBoxButtons.Cancel) == MessageBoxButtons.Cancel)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
        }

        /// <summary>
        /// Get the Visibility property for the Close button.
        /// </summary>
        public Visibility ButtonCloseVisibility
        {
            get
            {
                if ((Configuration.ButtonFlags & MessageBoxButtons.Close) == MessageBoxButtons.Close)
                {
                    return Visibility.Visible;
                }
                return Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Get the Visibility property for the Ignore button.
        /// </summary>
        public Visibility ButtonIgnoreVisibility
        {
            get
            {
                if ((Configuration.ButtonFlags & MessageBoxButtons.Ignore) == MessageBoxButtons.Ignore)
                {
                    return Visibility.Visible;
                }
                return Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Get the Visibility property for the Ok button.
        /// </summary>
        public Visibility ButtonOkVisibility
        {
            get
            {
                if ((Configuration.ButtonFlags & MessageBoxButtons.Ok) == MessageBoxButtons.Ok)
                {
                    return Visibility.Visible;
                }
                return Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Get the Visibility property for the Retry button.
        /// </summary>
        public Visibility ButtonRetryVisibility
        {
            get
            {
                if ((Configuration.ButtonFlags & MessageBoxButtons.Retry) == MessageBoxButtons.Retry)
                {
                    return Visibility.Visible;
                }
                return Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Get the Visibility property for the Yes button.
        /// </summary>
        public Visibility ButtonYesVisibility
        {
            get
            {
                if ((Configuration.ButtonFlags & MessageBoxButtons.Yes) == MessageBoxButtons.Yes)
                {
                    return Visibility.Visible;
                }
                return Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Get the Visibility property for the No button.
        /// </summary>
        public Visibility ButtonNoVisibility
        {
            get
            {
                if ((Configuration.ButtonFlags & MessageBoxButtons.No) == MessageBoxButtons.No)
                {
                    return Visibility.Visible;
                }
                return Visibility.Collapsed;
            }
        }
        #endregion button visibility

        #region Configuration
        /// <summary>
        /// Get or set the MessageBoxConfiguration options that control the behavior of the message-box.
        /// </summary>
        public MessageBoxConfiguration Configuration
        {
            get
            {
                if (_configuration == null)
                {
                    _configuration = new MessageBoxConfiguration();
                }
                return _configuration;
            }
            set
            {
                _configuration = value;
                // Cause this to recreate itself the next time the IconImage property is retrieved.
                _iconBitmapImage = null;
            }
        }
        #endregion

        #region DefaultConfiguration
        /// <summary>
        /// Get or set the MessageBoxDefaultConfiguration options that dictate the default appearance of the message-box.
        /// </summary>
        public MessageBoxDefaultConfiguration DefaultConfiguration
        {
            get
            {
                if (_defaultConfiguration == null)
                {
                    _defaultConfiguration = new MessageBoxDefaultConfiguration();
                }
                return _defaultConfiguration;
            }
            set
            {
                _defaultConfiguration = value;
            }
        }
        #endregion

        #region Height
        /// <summary>
        /// Get the height to use for the message-box Window.
        /// </summary>
        public double Height
        {
            get
            {
                double windowHeight = 155;
                if (StringLib.HasSomething(this.DetailText))
                {
                    // there is some detail-text
                    if (DetailText.Length > 87 || SummaryText.Length > 60)
                    {
                        windowHeight = 192;
                    }
                    // If the (main) text is long enough to warrant more than one line,
                    // make the text area left-justified instead of centered.
                    if (DetailText.Length > 122)
                    {
                        windowHeight = 220.0;
                    }
                }
                return windowHeight;
            }
        }
        #endregion

        #region SummaryText
        /// <summary>
        /// Get or set the text that's shown in the upper, summary-text area.
        /// </summary>
        public string SummaryText
        {
            get
            {
                if (StringLib.HasNothing(_summaryText))
                {
                    return String.Empty;
                }
                else
                {
                    return _summaryText;
                }
            }
            set
            {
                if (value != _summaryText)
                {
                    _summaryText = value;
                    Notify("SummaryText");
                }
            }
        }
        #endregion

        #region SummaryTextColor
        /// <summary>
        /// Get or set the Brush to use for the foreground color of the summary-text. Default is 003399, which is blue-ish.
        /// If you set this, then that color is always used from then on. Otherwise it is chosen by algorithem.
        /// </summary>
        public Brush SummaryTextColor
        {
            get
            {
                // If this has been explicitly set, then just use that color.
                if (_summaryTextColorExplicitlySet != null)
                {
                    return _summaryTextColorExplicitlySet;
                }
                else
                {
                    Brush brush = new SolidColorBrush(Colors.Black);
                    var texture = MessageBox.DefaultConfiguration.BackgroundTexture;
                    switch (_configuration.MessageType)
                    {
                        case MessageBoxType.SecuritySuccess:
                            if (texture == MessageBoxBackgroundTexturePreset.BrushedMetal)
                            {
                                brush = new SolidColorBrush(Colors.LightGreen);
                            }
                            else if (texture == MessageBoxBackgroundTexturePreset.BlueMarble)
                            {
                                brush = new SolidColorBrush(Colors.Blue);
                            }
                            else if (texture == MessageBoxBackgroundTexturePreset.BrownMarble1)
                            {
                                brush = new SolidColorBrush(Colors.Blue);
                            }
                            else
                            {
                                // With no background texture being used - it has only the green gradiant, so the best color here is white.
                                if (IsDarkBackground)
                                {
                                    brush = new SolidColorBrush(Colors.White);
                                }
                            }
                            break;
                        case MessageBoxType.UserMistake:
                            if (IsDarkBackground)
                            {
                                brush = new SolidColorBrush(Colors.White);
                            }
                            else
                            {
                                brush = new SolidColorBrush(Colors.MediumBlue);
                            }
                            break;
                        case MessageBoxType.Warning:
                            if (IsDarkBackground)
                            {
                                brush = new SolidColorBrush(Colors.White);
                            }
                            else
                            {
                                brush = new SolidColorBrush(Colors.OrangeRed);
                            }
                            break;
                        case MessageBoxType.Error:
                            if (IsDarkBackground)
                            {
                                brush = new SolidColorBrush(Colors.White);
                            }
                            else
                            {
                                brush = new SolidColorBrush(Colors.Red);
                            }
                            break;
                        case MessageBoxType.Stop:
                            if (IsDarkBackground)
                            {
                                brush = new SolidColorBrush(Colors.White);
                            }
                            else
                            {
                                brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF3E29"));
                            }
                            break;
                        default:
                            if (IsDarkBackground)
                            {
                                brush = new SolidColorBrush(Colors.White);
                            }
                            else
                            {
                                brush = new SolidColorBrush(Colors.Black);
                            }
                            break;
                    } // end switch.
                    return brush;
                }
            }
            set
            {
                if (!value.Equals(_summaryTextColorExplicitlySet))
                {
                    _summaryTextColorExplicitlySet = value;
                    Notify("SummaryTextColor");
                }
            }
        }
        #endregion

        #region SummaryTextBackground
        /// <summary>
        /// Get the Brush to use for the background of the summary text. Default is Transparent.
        /// </summary>
        public Brush SummaryTextBackground
        {
            get
            {
                string sTextBackground1 = null;
                switch (this.DefaultConfiguration.BackgroundTexture)
                {
                    case MessageBoxBackgroundTexturePreset.BlueTexture:
                        sTextBackground1 = "#99AECA";
                        break;
                    case MessageBoxBackgroundTexturePreset.BlueMarble:
                        //    sTextBackground1 = "#D2DDE3";
                        //    sTextBackground2 = "#D2DDE3";
                        break;
                    case MessageBoxBackgroundTexturePreset.BrownMarble1:
                        //sTextBackground1 = "#F7F7EF";
                        //sTextBackground2 = "#F7F7EF";
                        break;
                    case MessageBoxBackgroundTexturePreset.GrayMarble:
                        //sTextBackground1 = "#C0BBB8";
                        //txtText.Background = (SolidColorBrush)bc.ConvertFromString("#C0BBB8");
                        break;
                    case MessageBoxBackgroundTexturePreset.GrayTexture1:
                        //sTextBackground1 = "#BBBBBB";
                        //sTextBackground2 = "#ABABAB";
                        break;
                    case MessageBoxBackgroundTexturePreset.GrayTexture2:
                        sTextBackground1 = "#BBBBBB";
                        break;
                    case MessageBoxBackgroundTexturePreset.GreenTexture3:
                        //sTextBackground1 = "#A4B487";
                        //sTextBackground2 = "#A4B487";
                        sTextBackground1 = "#8E9E71";
                        break;
                }

#if !SILVERLIGHT
                //var bc = new System.Windows.Media.BrushConverter();
#endif
                if (sTextBackground1 == null)
                {
                    return Brushes.Transparent;
                }
                else
                {
                    //CBL  Which of these is better?
                    //summaryTextBackground = (SolidColorBrush)bc.ConvertFromString(sTextBackground1);
                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8E9E71"));
                }
            }
        }
        #endregion

        #region SummaryTextHorizontalContentAlignment
        /// <summary>
        /// Get the value to use for the HorizontalContentAlignment property of the summary-text. Default is Center.
        /// </summary>
        public HorizontalAlignment SummaryTextHorizontalContentAlignment
        {
            get
            {
                HorizontalAlignment alignment;
                // 77 is a reasonable number of characters if you only want this to shift to the left after two lines of text are filled.
                if (SummaryText.Length > 87)
                {
                    alignment = HorizontalAlignment.Left;
                }
                else if (SummaryText.Length > 34)
                {
                    // Shift it over partway, so it is still at least somewhat centered.
                    //_summaryTextMargin = new Thickness(minLeftMargin + 20, 0, 0, 0);
                    alignment = HorizontalAlignment.Center;
                }
                else
                {
                    alignment = HorizontalAlignment.Center;
                }
                return alignment;
            }
        }
        #endregion

        #region SummaryTextMargin
        /// <summary>
        /// Get the value to use for the message-box's "summary text" area Margin property.
        /// </summary>
        public Thickness SummaryTextMargin
        {
            get
            {
                // 77 is a reasonable number of characters if you only want this to shift to the left after two lines of text are filled.
                double iconMarginLeft = this.IconMargin.Left;
                Thickness summaryTextMargin = new Thickness(100, 0, 0, 0);
                if (SummaryText.Length > 87)
                {
                    summaryTextMargin = new Thickness(iconMarginLeft + IconWidth, 0, 0, 0);
                }
                else if (SummaryText.Length > 34)
                {
                    // Shift it over partway, so it is still at least somewhat centered.
                    //_summaryTextMargin = new Thickness(minLeftMargin + 20, 0, 0, 0);
                    summaryTextMargin = new Thickness(iconMarginLeft + IconWidth, 0, 0, 0);
                }
                else
                {
                    if (DetailText.Length < 40)
                    {
                        summaryTextMargin = new Thickness(0, 0, 0, 0);
                    }
                    else
                    {
                        summaryTextMargin = new Thickness(iconMarginLeft + IconWidth, 0, 0, 0);
                    }
                }
                return summaryTextMargin;
            }
        }
        #endregion

        #region SummaryTextRowSpan
        /// <summary>
        /// Get the value to use for the message-box's "summary text" TextBox Grid.RowSpan attached-property.
        ///  Usually it is 1, but we set it to 2 if there is no DetailText.
        /// </summary>
        public int SummaryTextRowSpan
        {
            get
            {
                int summaryTextRowSpan = 1;
                // If there is no detail-text,
                if (StringLib.HasNothing(_detailText))
                {
                    // then let the summary-text span the entire height that is available for text.
                    summaryTextRowSpan = 2;
                }
                else // there is some detail-text
                {
                    summaryTextRowSpan = 1;
                }
                return summaryTextRowSpan;
            }
        }
        #endregion

        #region DetailText
        /// <summary>
        /// Get or set the text to be shown to the user. This never returns null; if there is none then String.Empty is returned.
        /// </summary>
        public string DetailText
        {
            get
            {
                if (StringLib.HasNothing(_detailText))
                {
                    return String.Empty;
                }
                else
                {
                    return _detailText;
                }
            }
            set
            {
                //Console.WriteLine("VM.set_DetailText, value is " + StringLib.AsNonNullString(_detailText));
                if (value != _detailText)
                {
                    _detailText = value;
                    Notify("DetailText");
                }
            }
        }
        #endregion

        #region DetailTextBackground
        /// <summary>
        /// Get the Brush to use for the background of the DetailText. Default is Transparent.
        /// </summary>
        public Brush DetailTextBackground
        {
            get
            {
                string sTextBackground2 = null;
                switch (MessageBox.DefaultConfiguration.BackgroundTexture)
                {
                    case MessageBoxBackgroundTexturePreset.BlueTexture:
                        sTextBackground2 = "#99AECA";
                        break;
                    case MessageBoxBackgroundTexturePreset.BlueMarble:
                        //    sTextBackground1 = "#D2DDE3";
                        //    sTextBackground2 = "#D2DDE3";
                        break;
                    case MessageBoxBackgroundTexturePreset.BrownMarble1:
                        //sTextBackground1 = "#F7F7EF";
                        //sTextBackground2 = "#F7F7EF";
                        break;
                    case MessageBoxBackgroundTexturePreset.GrayMarble:
                        //sTextBackground1 = "#C0BBB8";
                        //txtText.Background = (SolidColorBrush)bc.ConvertFromString("#C0BBB8");
                        break;
                    case MessageBoxBackgroundTexturePreset.GrayTexture1:
                        //sTextBackground1 = "#BBBBBB";
                        //sTextBackground2 = "#ABABAB";
                        break;
                    case MessageBoxBackgroundTexturePreset.GrayTexture2:
                        sTextBackground2 = "#BBBBBB";
                        break;
                    case MessageBoxBackgroundTexturePreset.GreenTexture3:
                        //sTextBackground1 = "#A4B487";
                        //sTextBackground2 = "#A4B487";
                        sTextBackground2 = "#8E9E71";
                        break;
                }

#if !SILVERLIGHT
                //var bc = new System.Windows.Media.BrushConverter();
#endif
                if (sTextBackground2 == null)
                {
                    return Brushes.Transparent;
                }
                else
                {
                    //CBL  Which of these two methods is better?
                    //detailTextBackground = (SolidColorBrush)bc.ConvertFromString(sTextBackground2);
                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString(sTextBackground2));
                }
            }
        }
        #endregion

        #region DetailTextHorizontalContentAlignment
        /// <summary>
        /// Get the value to use for the HorizontalContentAlignment property of the DetailText.
        /// </summary>
        public HorizontalAlignment DetailTextHorizontalContentAlignment
        {
            get
            {
                // If the detail-text is long enough such that to center it no longer makes sense, set it to left-justify.
                if (DetailText.Length > 100)
                {
                    return HorizontalAlignment.Left;
                }
                return HorizontalAlignment.Center;
            }
        }
        #endregion

        #region DetailTextVerticalAlignment
        /// <summary>
        /// Get the value to use for the VerticalAlignment property of the DetailText. Default is Center.
        /// </summary>
        public VerticalAlignment DetailTextVerticalAlignment
        {
            get
            {
                // If there is detail-text,
                if (StringLib.HasSomething(this.DetailText))
                {
                    // If the (main) text is long enough to warrant more than one line,
                    // make the text area aligned against the top instead of centered.
                    if (DetailText.Length > 122)
                    {
                        return VerticalAlignment.Top;
                    }
                }
                return VerticalAlignment.Center;
            }
        }
        #endregion

        #region DetailTextMargin
        /// <summary>
        /// Get the value to use for the message-box's "detail text" area Margin property.
        /// </summary>
        public Thickness DetailTextMargin
        {
            get
            {
                double detailTextMarginLeft = 0.0;
                double minLeftMargin = 39;
                // Determine whether the icon extends into the bottom, detail-text region..
                bool isIconExtendingIntoDetailArea = (this.IconHeight > 38) ? true : false;

                if (IconWidth > 32)
                {
                    minLeftMargin = IconWidth + 2;
                }
                else if (IconWidth == 0)
                {
                    minLeftMargin = 0;
                }
                else
                {
                    minLeftMargin = IconWidth - 4;
                }
                if (SummaryText.Length <= 34)
                {
                    if (DetailText.Length < 40)
                    {
                        detailTextMarginLeft = 0;
                    }
                }
                //CBL Clearly this needs simplifying.
                detailTextMarginLeft = minLeftMargin;
                // If there is some detail-text,
                if (StringLib.HasSomething(this.DetailText))
                {
                    // If the detail-text is too long to fit on one line, expand it to the left so that it fills the width of the entire message-box.
                    if (isIconExtendingIntoDetailArea)
                    {
                        if (SummaryText.Length < 34 && DetailText.Length < 40)
                        {
                            detailTextMarginLeft = 0;
                        }
                        else
                        {
                            detailTextMarginLeft = minLeftMargin;
                        }
                    }
                }
                return new Thickness(detailTextMarginLeft, 0, 0, 0);
            }
        }
        #endregion

        #region the gradiant-color rectangle

        #region GradiantLeftColor
        /// <summary>
        /// Get the color for the left edge of the color gradiant that underlies the upper portion of the message-box.
        /// </summary>
        public Color GradiantLeftColor
        {
            get
            {
                // If there is no background-texture, then - 
                if (this.DefaultConfiguration.BackgroundTexture == MessageBoxBackgroundTexturePreset.None)
                {
                    // Select the left-most color of the gradiant, based upon the type of message..
                    switch (_configuration.MessageType)
                    {
                        case MessageBoxType.Error:
                            // This originally was a red gradient that goes from #AC0100 at the left, to #E30100 at the right.
                            return Color.FromRgb(0xEB, 0x01, 0x00);
                        case MessageBoxType.SecurityIssue:
                            // A yellow gradiant, #F2B100 at the left, to #FECD48 at the right.
                            return Color.FromRgb(0xF2, 0xB1, 0x00);
                        case MessageBoxType.SecuritySuccess:
                            // This is a gradient that goes from #157615 at the left, to #39963F at the right.
                            return Color.FromRgb(0x15, 0x76, 0x15);
                        case MessageBoxType.Stop:
                            return Colors.Red;
                    }
                }
                return default(Color);
            }
        }
        #endregion

        #region GradiantRightColor
        /// <summary>
        /// Get the color for the right edge of the color gradiant that underlies the upper portion of the message-box.
        /// </summary>
        public Color GradiantRightColor
        {
            get
            {
                // If there is no background-texture, then -
                if (this.DefaultConfiguration.BackgroundTexture == MessageBoxBackgroundTexturePreset.None)
                {
                    // Select the left-most color of the gradiant, based upon the type of message..
                    switch (_configuration.MessageType)
                    {
                        case MessageBoxType.Error:
                            // This is a red gradient that goes from #AC0100 at the left, to #E30100 at the right.
                            return Colors.LightCoral;
                        case MessageBoxType.SecurityIssue:
                            // A yellow gradiant, #F2B100 at the left, to #FECD48 at the right.
                            return Color.FromRgb(0xFE, 0xCD, 0x48);
                        case MessageBoxType.SecuritySuccess:
                            // This is a gradient that goes from #157615 at the left, to #39963F at the right.
                            return Color.FromRgb(0x7C, 0xC7, 0x6B);
                        case MessageBoxType.Stop:
                            return Colors.DarkOrange;
                    }
                }
                return default(Color);
            }
        }
        #endregion

        #region IsGradiantShown
        /// <summary>
        /// Get or set whether to show the color-gradiant rectangle. True by default.
        /// </summary>
        public bool IsGradiantShown
        {
            get { return _isGradiantShown; }
            set
            {
                if (value != _isGradiantShown)
                {
                    _isGradiantShown = value;
                    Notify("UpperRectangleVisibility");
                }
            }
        }
        #endregion

        #region UpperRectangleMargin
        /// <summary>
        /// Get the value to use for the Margin property of the Rectangle that contains the color-grandiant,
        /// which depends upon the message type and which icons are used.
        /// </summary>
        public Thickness UpperRectangleMargin
        {
            get
            {
                if (_configuration.MessageType == MessageBoxType.Stop)
                {
                    if (MessageBox.DefaultConfiguration.IsUsingNewerIcons)
                    {
                        return new Thickness(68, 0, 0, 0);
                    }
                    else
                    {
                        return new Thickness(45, 0, 0, 0);
                    }
                }
                else
                {
                    return new Thickness(0, 0, 0, 0); ;
                }
            }
        }
        #endregion

        #region UpperRectangleRowSpan
        /// <summary>
        /// Get the value for the Grid.RowSpan property of the Rectangle visual element.
        /// Default is 1, but it changes to 2 if there is no detail-text.
        /// </summary>
        public int UpperRectangleRowSpan
        {
            get
            {
                // If there is no detail-text,
                if (StringLib.HasNothing(_detailText))
                {
                    // then let the summary-text span the entire height that is available for text.
                    return 2;
                }
                else // there is some detail-text
                {
                    return 1;
                }
            }
        }
        #endregion

        #region UpperRectangleVisibility
        /// <summary>
        /// Get the visibility-value of the Rectangle that underlies the upper half of the interior of the message-box.
        /// </summary>
        public Visibility UpperRectangleVisibility
        {
            get
            {
                Visibility upperRectangleVisibility;
                // If the gradiant is turned off or there is a background-texture, then don't show it.
                if (!_isGradiantShown || this.DefaultConfiguration.BackgroundTexture != MessageBoxBackgroundTexturePreset.None)
                {
                    upperRectangleVisibility = Visibility.Collapsed;
                }
                else
                {
                    // These specific message types have a gradiant defined for them.
                    switch (_configuration.MessageType)
                    {
                        case MessageBoxType.SecurityIssue:
                            upperRectangleVisibility = Visibility.Visible;
                            break;
                        case MessageBoxType.SecuritySuccess:
                            upperRectangleVisibility = Visibility.Visible;
                            break;
                        case MessageBoxType.Error:
                            upperRectangleVisibility = Visibility.Visible;
                            break;
                        case MessageBoxType.Stop:
                            upperRectangleVisibility = Visibility.Visible;
                            break;
                        default:
                            // All the rest have no rectangle.
                            upperRectangleVisibility = Visibility.Collapsed;
                            break;
                    }
                }
                return upperRectangleVisibility;
            }
        }
        #endregion

        #endregion the gradiant-color rectangle

        #region IconImage
        /// <summary>
        /// Get the BitmapImage to use for the message-box's icon.
        /// </summary>
        public BitmapImage IconImage
        {
            get
            {
                //Console.WriteLine("get_IconImage, " + _configuration.MessageType.ToString()");
                //Console.WriteLine("  after init, _iconBitmapImage is " + StringLib.AsNonNullString(_iconBitmapImage));
                //Console.WriteLine("  and         _iconImageName is " + StringLib.AsNonNullString(_iconImageName));
                if (_iconBitmapImage == null)
                {
                    // If no image-name has been assigned, then produce no icon-image.
                    if (IconFilename != null)
                    {
#if SILVERLIGHT
                        string stringForUri = @"images/" + IconFilename;
                        _bitmapImageForIcon = new BitmapImage(new Uri(stringForUri, UriKind.Relative));
#else
                        string stringForUri = @"pack://application:,,,/Hurst.BaseLibWpf;Component/images/" + IconFilename;
                        _iconBitmapImage = new BitmapImage(new Uri(stringForUri));
#endif
                        //Console.WriteLine("***Creating new icon BitmapImage!***"); //CBL
                    }
                }
                return _iconBitmapImage;
            }
        }
        #endregion

        #region IconMargin
        /// <summary>
        /// Get the value to use for the message-box's icon's Margin property.
        /// </summary>
        public Thickness IconMargin
        {
            get
            {
                int iconMarginLeft = 8;
                int iconMarginTop = 5;
                int iconMarginRight = 8;
                int iconMarginBottom = 0;

                switch (_configuration.MessageType)
                {
                    case MessageBoxType.Error:
                        if (_defaultConfiguration.IsUsingNewerIcons)
                        {
                            iconMarginLeft = 1;
                        }
                        else // use the old icon
                        {
                            iconMarginLeft = 3;
                        }
                        iconMarginRight = iconMarginBottom = 0;
                        break;
                    case MessageBoxType.Information:
                        if (_defaultConfiguration.IsUsingNewerIcons)
                        {
                            iconMarginLeft = 0;
                        }
                        else // use the old icon
                        {
                            iconMarginLeft = 4;
                        }
                        iconMarginRight = iconMarginBottom = 0;
                        break;
                    case MessageBoxType.None:
                        iconMarginLeft = iconMarginTop = iconMarginRight = iconMarginBottom = 0;
                        break;
                    case MessageBoxType.Question:
                        if (_defaultConfiguration.IsUsingNewerIcons)
                        {
                            iconMarginLeft = 0;
                        }
                        else // use the old icon
                        {
                            iconMarginLeft = 4;
                        }
                        iconMarginRight = iconMarginBottom = 0;
                        break;
                    case MessageBoxType.SecurityIssue:
                        iconMarginRight = 0;
                        break;
                    case MessageBoxType.SecuritySuccess:
                        iconMarginRight = 0;
                        break;
                    case MessageBoxType.Stop:
                        if (_defaultConfiguration.IsUsingNewerIcons)
                        {
                            iconMarginLeft = 0;
                        }
                        else // use the old icon
                        {
                            iconMarginLeft = 5;
                        }
                        iconMarginRight = iconMarginBottom = 0;
                        break;
                    case MessageBoxType.UserMistake:
                        if (_defaultConfiguration.IsUsingNewerIcons)
                        {
                            iconMarginLeft = 0;
                        }
                        else // use the old icon
                        {
                            iconMarginLeft = 4;
                        }
                        iconMarginRight = iconMarginBottom = 0;
                        break;
                    case MessageBoxType.Warning:
                        if (_defaultConfiguration.IsUsingNewerIcons)
                        {
                            iconMarginLeft = 3;
                        }
                        else // use the old icon
                        {
                            iconMarginLeft = 4;
                        }
                        iconMarginRight = iconMarginBottom = 0;
                        break;
                    default:
                        iconMarginLeft = iconMarginTop = iconMarginRight = iconMarginBottom = 0;
                        break;
                }

                int iconHeight = this.IconHeight;
                if (iconHeight > 38)
                {
                    if (iconHeight <= 48)
                    {
                        iconMarginTop = 5;
                    }
                    else
                    {
                        iconMarginTop = 0;
                    }
                }
                else
                {
                    iconMarginLeft = 9;
                    iconMarginTop = 36 - iconHeight;
                }
                return new Thickness(iconMarginLeft, iconMarginTop, iconMarginRight, iconMarginBottom);
            }
        }
        #endregion

        #region IsCustomElementVisible
        /// <summary>
        /// Get or set whether to display gridCustomElement,
        /// which would be expected to contain the externally-supplied UIElement.
        /// </summary>
        public bool IsCustomElementVisible
        {
            get { return _isCustomElementVisible; }
            set
            {
                //CBL  Does this really make sense?
                if (value != _isCustomElementVisible)
                {
                    _isCustomElementVisible = value;
                    Notify("IsCustomElementVisible");
                }
            }
        }
        #endregion

        #region Title
        /// <summary>
        /// Get or set the text to be displayed in the message-box Window's title-bar.
        /// </summary>
        public string Title
        {
            get
            {
                if (IsInDesignMode)
                {
                    return _title + " (MessageType is " + this.Configuration.MessageType + ")";
                }
                else
                {
                    if (_title == null)
                    {
                        return "A null title!";  //TODO
                    }
                    return _title;
                }
            }
            set
            {
                if (value != _title)
                {
                    _title = value;
                    Notify("Title");
                }
            }
        }
        #endregion

        #endregion public properties

        #region internal implementation

        //CBL  Added this. Trying to update how the view-model works.
        #region Init
        /// <summary>
        /// Initialize this object's properties from the given manager and options.
        /// Call this before the MessageBoxWindow is loaded (ie, the Loaded event gets raised).
        /// </summary>
        /// <param name="manager">the MessageBox object that serves as a manager of the message-box facility</param>
        /// <param name="options">a MessageBoxConfiguration to get the options from</param>
        internal void Init(MessageBox manager, MessageBoxConfiguration options)
        {
            Configuration = options;
            DetailText = options.DetailText;
            SummaryText = options.SummaryText;
            // If there is no caption, then just put the caption-prefix without the spacer.
            if (StringLib.HasNothing(options.CaptionAfterPrefix))
            {
                if (StringLib.HasNothing(options.CaptionPrefix))
                {
                    Title = manager.CaptionPrefix;
                }
                else
                {
                    Title = options.CaptionPrefix;
                }
            }
            else // the caption-suffix is not empty.
            {
                if (StringLib.HasNothing(options.CaptionPrefix))
                {
                    Title = manager.CaptionPrefix + ": " + options.CaptionAfterPrefix;
                }
                else
                {
                    Title = options.CaptionPrefix + ": " + options.CaptionAfterPrefix;
                }
            }
            // Cause all of the property binding values to be retrieved again
            // that depend upon the _configuration choices.
            Notify("BackgroundBrush");
            Notify("ButtonMargin");
            Notify("ButtonCount");
            Notify("ButtonWidth");
            Notify("ButtonPanelBackground");
            Notify("ButtonCancelVisibility");
            Notify("ButtonCloseVisibility");
            Notify("ButtonIgnoreVisibility");
            Notify("ButtonOkVisibility");
            Notify("ButtonRetryVisibility");
            Notify("ButtonYesVisibility");
            Notify("ButtonNoVisibility");
            Notify("Height");
            Notify("SummaryTextColor");
            Notify("SummaryTextBackground");
            Notify("SummaryTextHorizontalContentAlignment");
            Notify("SummaryTextMargin");
            Notify("SummaryTextRowSpan");
            Notify("DetailTextBackground");
            Notify("DetailTextHorizontalContentAlignment");
            Notify("DetailTextVerticalAlignment");
            Notify("DetailTextMargin");
            Notify("GradiantLeftColor");
            Notify("GradiantRightColor");
            Notify("IconImage");
            Notify("IconMargin");
            Notify("UpperRectangleRowSpan");
            Notify("UpperRectangleVisibility");
        }
        #endregion

        #region GetNumberOfButtons
        /// <summary>
        /// Get the number of buttons that we want to be visible.
        /// </summary>
        /// <returns>the number of the pre-defined buttons that the Configuration indicates as being visible</returns>
        private int GetNumberOfButtons()
        {
            int buttonCount = 0;
            if ((Configuration.ButtonFlags & MessageBoxButtons.Cancel) == MessageBoxButtons.Cancel)
            {
                buttonCount++;
            }
            if ((Configuration.ButtonFlags & MessageBoxButtons.Close) == MessageBoxButtons.Close)
            {
                buttonCount++;
            }
            if ((Configuration.ButtonFlags & MessageBoxButtons.Ignore) == MessageBoxButtons.Ignore)
            {
                buttonCount++;
            }
            if ((Configuration.ButtonFlags & MessageBoxButtons.Ok) == MessageBoxButtons.Ok)
            {
                buttonCount++;
            }
            if ((Configuration.ButtonFlags & MessageBoxButtons.Retry) == MessageBoxButtons.Retry)
            {
                buttonCount++;
            }
            if ((Configuration.ButtonFlags & MessageBoxButtons.Yes) == MessageBoxButtons.Yes)
            {
                buttonCount++;
            }
            if ((Configuration.ButtonFlags & MessageBoxButtons.No) == MessageBoxButtons.No)
            {
                buttonCount++;
            }
            return buttonCount;
        }
        #endregion

        #region IconFilename
        /// <summary>
        /// Get the name of the bitmap file to use for the message-box's icon (just the filename itself - not the entire path).
        /// Null is returned if there is none.
        /// </summary>
        private string IconFilename
        {
            get
            {
                string iconImageName = null;
                switch (_configuration.MessageType)
                {
                    case MessageBoxType.Error:
                        if (_defaultConfiguration.IsUsingNewerIcons)
                        {
                            iconImageName = "errorDiamond_48x48.png";
                        }
                        else // use the old icon
                        {
                            iconImageName = "RedShield.png";
                        }
                        break;
                    case MessageBoxType.Information:
                        if (_defaultConfiguration.IsUsingNewerIcons)
                        {
                            iconImageName = "InfoBlueDiamond_64x64.png";
                        }
                        else // use the old icon
                        {
                            iconImageName = "iconInformationStd.png";
                        }
                        break;
                    case MessageBoxType.Question:
                        if (_defaultConfiguration.IsUsingNewerIcons)
                        {
                            iconImageName = "QDiamond_64x64.png";
                        }
                        else // use the old icon
                        {
                            iconImageName = "iconQuestionStd.png";
                        }
                        break;
                    case MessageBoxType.SecurityIssue:
                        iconImageName = "SecurityWarning.png";
                        break;
                    case MessageBoxType.SecuritySuccess:
                        iconImageName = "SecuritySuccess.png";
                        break;
                    case MessageBoxType.Stop:
                        if (_defaultConfiguration.IsUsingNewerIcons)
                        {
                            iconImageName = "hand64x64.png";
                        }
                        else // use the old icon
                        {
                            iconImageName = "iconErrorStd.gif";
                        }
                        break;
                    case MessageBoxType.UserMistake:
                        if (_defaultConfiguration.IsUsingNewerIcons)
                        {
                            //TODO: This image needs to have it's background transparency adjusted.
                            iconImageName = "Oops.png";
                        }
                        else // use the old icon
                        {
                            iconImageName = "iconWarningStd.gif";
                        }
                        break;
                    case MessageBoxType.Warning:
                        if (_defaultConfiguration.IsUsingNewerIcons)
                        {
                            iconImageName = "warning3_64x64.png";
                        }
                        else // use the old icon
                        {
                            iconImageName = "iconWarningStd.gif";
                        }
                        break;
                }
                return iconImageName;
            }
        }
        #endregion

        #region IconHeight
        /// <summary>
        /// Get the width of the icon-symbol that is displayed on the left.
        /// </summary>
        public int IconHeight
        {
            get
            {
                int iconHeight = 32;
                switch (_configuration.MessageType)
                {
                    case MessageBoxType.Error:
                        if (_defaultConfiguration.IsUsingNewerIcons)
                        {
                            iconHeight = 48;
                        }
                        break;
                    case MessageBoxType.Information:
                        if (_defaultConfiguration.IsUsingNewerIcons)
                        {
                            iconHeight = 64;
                        }
                        break;
                    case MessageBoxType.Question:
                        if (_defaultConfiguration.IsUsingNewerIcons)
                        {
                            iconHeight = 82;
                        }
                        break;
                    case MessageBoxType.SecurityIssue:
                        iconHeight = 33;
                        break;
                    case MessageBoxType.SecuritySuccess:
                        iconHeight = 30;
                        break;
                    case MessageBoxType.Stop:
                        if (_defaultConfiguration.IsUsingNewerIcons)
                        {
                            iconHeight = 64;
                        }
                        break;
                    case MessageBoxType.UserMistake:
                        if (_defaultConfiguration.IsUsingNewerIcons)
                        {
                            iconHeight = 65;
                        }
                        else // use the old icon
                        {
                            iconHeight = 28;
                        }
                        break;
                    case MessageBoxType.Warning:
                        if (_defaultConfiguration.IsUsingNewerIcons)
                        {
                            iconHeight = 64;
                        }
                        else // use the old icon
                        {
                            iconHeight = 28;
                        }
                        break;
                }
                return iconHeight;
            }
        }
        #endregion IconHeight

        #region IconWidth
        /// <summary>
        /// Get the width of the icon-symbol that is displayed on the left.
        /// </summary>
        private int IconWidth
        {
            get
            {
                int iconWidth = 0;
                switch (_configuration.MessageType)
                {
                    case MessageBoxType.SecurityIssue:
                        iconWidth = 26;
                        break;
                    case MessageBoxType.SecuritySuccess:
                        iconWidth = 26;
                        break;
                    case MessageBoxType.Information:
                        if (_defaultConfiguration.IsUsingNewerIcons)
                        {
                            iconWidth = 64;
                        }
                        else // use the old icon
                        {
                            iconWidth = 32;
                        }
                        break;
                    case MessageBoxType.Question:
                        if (_defaultConfiguration.IsUsingNewerIcons)
                        {
                            iconWidth = 82;
                        }
                        else // use the old icon
                        {
                            iconWidth = 32;
                        }
                        break;
                    case MessageBoxType.UserMistake:
                        if (_defaultConfiguration.IsUsingNewerIcons)
                        {
                            iconWidth = 65;
                        }
                        else // use the old icon
                        {
                            iconWidth = 31;
                        }
                        break;
                    case MessageBoxType.Warning:
                        if (_defaultConfiguration.IsUsingNewerIcons)
                        {
                            iconWidth = 64;
                        }
                        else // use the old icon
                        {
                            iconWidth = 31;
                        }
                        break;
                    case MessageBoxType.Error:
                        if (_defaultConfiguration.IsUsingNewerIcons)
                        {
                            iconWidth = 48;
                        }
                        else // use the old icon
                        {
                            iconWidth = 26;
                        }
                        break;
                    case MessageBoxType.Stop:
                        if (_defaultConfiguration.IsUsingNewerIcons)
                        {
                            iconWidth = 64;
                        }
                        else // use the old icon
                        {
                            iconWidth = 32;
                        }
                        break;
                }
                return iconWidth;
            }
        }
        #endregion IconWidth

        #region IsDarkBackground
        /// <summary>
        /// Get whether the selected background for this message-box would be considered a 'dark' color
        /// for the purpose of determining an appropriate color for the text -
        /// based upon the MessageType and BackgroundTexture.
        /// </summary>
        private bool IsDarkBackground
        {
            get
            {
                bool isConsideredDark = false;
                var texture = _defaultConfiguration.BackgroundTexture;
                switch (texture)
                {
                    case MessageBoxBackgroundTexturePreset.BlueMarble:
                        isConsideredDark = false;
                        break;
                    case MessageBoxBackgroundTexturePreset.BrownMarble1:
                        isConsideredDark = false;
                        break;
                    case MessageBoxBackgroundTexturePreset.BrownMarble2:
                        isConsideredDark = false;
                        break;
                    case MessageBoxBackgroundTexturePreset.BrownTexture1:
                        isConsideredDark = false;
                        break;
                    case MessageBoxBackgroundTexturePreset.BrownTexture2:
                        isConsideredDark = false;
                        break;
                    case MessageBoxBackgroundTexturePreset.BrushedMetal:
                        isConsideredDark = true;
                        break;
                    case MessageBoxBackgroundTexturePreset.GrayTexture1:
                        isConsideredDark = true;
                        break;
                    default:
                        // We exhausted the decision-points that are based upon the background texture.
                        // Now consider the message-type, which dictates the color of the background gradiant..
                        if (IsGradiantShown)
                        {
                            switch (_configuration.MessageType)
                            {
                                case MessageBoxType.Error:
                                    isConsideredDark = true;
                                    break;
                                case MessageBoxType.SecuritySuccess:
                                    // With no background texture being used - it has only the green gradiant, so the best color here is white.
                                    isConsideredDark = true;
                                    break;
                                case MessageBoxType.Stop:
                                    isConsideredDark = true;
                                    break;
                                default:
                                    isConsideredDark = false;
                                    break;
                            }
                        }
                        break;
                } // end switch.
                return isConsideredDark;
            }
        }
        #endregion

        #region fields

        private MessageBoxConfiguration _configuration;
        private MessageBoxDefaultConfiguration _defaultConfiguration;
        private string _detailText;
        /// <summary>
        /// the BitmapImage to use for the one optional icon-image that may be shown in the upper-left (not necessarily an "icon", technically).
        /// </summary>
        private BitmapImage _iconBitmapImage;
        private bool _isCustomElementVisible;
        private bool _isGradiantShown = true;
        /// <summary>
        /// When a color for the summary-text has been explicitly set, that color is stored in this.
        /// If this is null, then no color has been specified.
        /// </summary>
        private Brush _summaryTextColorExplicitlySet;
        private string _summaryText;
        private static MessageBoxViewModel _theInstance;
        /// <summary>
        /// the text to be displayed in the message-box Window's title-bar.
        /// </summary>
        private string _title;

        #endregion fields

        #endregion internal implementation
    }
    #endregion
}

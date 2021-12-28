#if PRE_4
#define PRE_5
#endif
using System;
using System.Windows;


namespace Hurst.BaseLibWpf.Display
{
    #region class MessageBoxConfiguration : IMessageBoxConfiguration
    /// <summary>
    /// This class encapsulates the instant parameter options that you may want to set on your MessageBox,
    /// in case you prefer to invoke it with one object containing all of the parameter values.
    /// This is apart from those argument values that are provided by MessageBoxDefaultConfiguration.
    /// </summary>
    /// <remarks>
    /// This was previously named MessageBoxOptions, which seemed the most obvious choice for the name
    /// but there was excessive gnashing of teeth over the naming conflict with the MessageBoxOptions enum.
    /// </remarks>
    public sealed class MessageBoxConfiguration : IMessageBoxConfiguration
    {
        //CBL Why is this here?
        public event EventHandler<MessageBoxCompletedEventArgs> Completed = delegate { };

        #region constructors
        /// <summary>
        /// Default Constructor: Create a new MessageBoxConfiguration object with the a MessageBoxType of None.
        /// </summary>
        public MessageBoxConfiguration()
        {
            this._messageType = MessageBoxType.None;
        }

        /// <summary>
        /// Create a new MessageBoxConfiguration object with the given MessageBoxType and default values.
        /// </summary>
        public MessageBoxConfiguration(MessageBoxType whichType)
            : this()
        {
            this._messageType = whichType;
        }

        /// <summary>
        /// The Copy-Constructor. This does NOT copy the SummaryText or DetailText.
        /// </summary>
        /// <param name="source">the MessageBoxConfiguration object to copy the values from</param>
        public MessageBoxConfiguration(MessageBoxConfiguration source)
        {
            this._buttonFlags = source._buttonFlags;
            //            this._captionPrefix = source.CaptionPrefix;
            this._captionAfterPrefix = source.CaptionAfterPrefix;
            this._isToCenterOverParent = source.IsToCenterOverParent;
            this._isToBeTopmostWindow = source.IsToBeTopmostWindow;
            this._messageType = source.MessageType;
            this._timeoutPeriodInSeconds = source.TimeoutPeriodInSeconds;
        }
        #endregion

        #region buttons
        /// <summary>
        /// Get or set the flags that dictate which buttons to show. The default is to just show the "Ok" button.
        /// </summary>
        public MessageBoxButtons ButtonFlags
        {
            get { return _buttonFlags; }
            set { _buttonFlags = value; }
        }

        /// <summary>
        /// Set the flags that dictate which buttons to show. The default is to just show the "Ok" button.
        /// </summary>
        /// <param name="buttons">A bitwise-combination of the enum flags defining which buttons to show</param>
        /// <returns>a reference to this same object such that other method-calls may be chained</returns>
        public MessageBoxConfiguration SetButtonFlags(MessageBoxButtons buttons)
        {
            _buttonFlags = buttons;
            return this;
        }

        public string GetButtonText(MessageResult forWhichResponse)
        {
            string buttonText = String.Empty;
            switch (forWhichResponse)
            {
                case MessageResult.Abort:
                    buttonText = _buttonAbortText;
                    break;
                case MessageResult.Cancel:
                    buttonText = _buttonCancelText;
                    break;
                case MessageResult.Close:
                    buttonText = _buttonCloseText;
                    break;
                case MessageResult.Ignore:
                    buttonText = _buttonIgnoreText;
                    break;
                case MessageResult.No:
                    buttonText = _buttonNoText;
                    break;
                case MessageResult.Ok:
                    buttonText = _buttonOkText;
                    break;
                case MessageResult.Retry:
                    buttonText = _buttonRetryText;
                    break;
                case MessageResult.Yes:
                    buttonText = _buttonYesText;
                    break;
                default:
                    break;
            }
            return buttonText;
        }

        public string ButtonAbortText
        {
            get { return _buttonAbortText; }
        }

        public string ButtonCancelText
        {
            get { return _buttonCancelText; }
        }

        public string ButtonCloseText
        {
            get { return _buttonCloseText; }
        }

        public string ButtonIgnoreText
        {
            get { return _buttonIgnoreText; }
        }

        public string ButtonNoText
        {
            get { return _buttonNoText; }
        }

        public string ButtonOkText
        {
            get { return _buttonOkText; }
        }

        public string ButtonRetryText
        {
            get { return _buttonRetryText; }
        }

        public string ButtonYesText
        {
            get { return _buttonYesText; }
        }

        public MessageBoxConfiguration SetButtonText(MessageResult forWhichResponse, string buttonText)
        {
            switch (forWhichResponse)
            {
                case MessageResult.Abort:
                    _buttonAbortText = buttonText;
                    break;
                case MessageResult.Cancel:
                    _buttonCancelText = buttonText;
                    break;
                case MessageResult.Close:
                    _buttonCloseText = buttonText;
                    break;
                case MessageResult.Ignore:
                    _buttonIgnoreText = buttonText;
                    break;
                case MessageResult.No:
                    _buttonNoText = buttonText;
                    break;
                case MessageResult.Ok:
                    _buttonOkText = buttonText;
                    break;
                case MessageResult.Retry:
                    _buttonRetryText = buttonText;
                    break;
                case MessageResult.Yes:
                    _buttonYesText = buttonText;
                    break;
                default:
                    break;
            }
            return this;
        }

        public string ButtonAbortToolTip
        {
            get { return _buttonAbortToolTipText; }
        }

        public string ButtonCancelToolTip
        {
            get { return _buttonCancelToolTipText; }
        }

        public string ButtonCloseToolTip
        {
            get { return _buttonCloseToolTipText; }
        }

        public string ButtonIgnoreToolTip
        {
            get { return _buttonIgnoreToolTipText; }
        }

        public string ButtonNoToolTip
        {
            get { return _buttonNoToolTipText; }
        }

        public string ButtonOkToolTip
        {
            get { return _buttonOkToolTipText; }
        }

        public string ButtonRetryToolTip
        {
            get { return _buttonRetryToolTipText; }
        }

        public string ButtonYesToolTip
        {
            get { return _buttonYesToolTipText; }
        }

        public MessageBoxConfiguration SetButtonToolTip(MessageResult forWhichResponse, string tooltipText)
        {
            switch (forWhichResponse)
            {
                case MessageResult.Abort:
                    _buttonAbortToolTipText = tooltipText;
                    break;
                case MessageResult.Cancel:
                    _buttonCancelToolTipText = tooltipText;
                    break;
                case MessageResult.Close:
                    _buttonCloseToolTipText = tooltipText;
                    break;
                case MessageResult.Ignore:
                    _buttonIgnoreToolTipText = tooltipText;
                    break;
                case MessageResult.No:
                    _buttonNoToolTipText = tooltipText;
                    break;
                case MessageResult.Ok:
                    _buttonOkToolTipText = tooltipText;
                    break;
                case MessageResult.Retry:
                    _buttonRetryToolTipText = tooltipText;
                    break;
                case MessageResult.Yes:
                    _buttonYesToolTipText = tooltipText;
                    break;
                default:
                    break;
            }
            return this;
        }
        #endregion buttons

        #region caption

        /// <summary>
        /// Get or set the initial text that's shown in the title-bar of the Window, which normally would be comprised of
        /// the vendor-name followed by the application-name. This is an override-value; it defaults to null.
        /// </summary>
        public string CaptionPrefix
        {
            //CBL: Does I want this to be an externally-exposed property?
            get { return _captionPrefix; }
            set { _captionPrefix = value; }
        }

        /// <summary>
        /// Get or set the text that's shown in the title-bar of the Window.
        /// </summary>
        public string CaptionAfterPrefix
        {
            get { return _captionAfterPrefix; }
            set { _captionAfterPrefix = value; }
        }

        /// <summary>
        /// Set the text that is to be shown (after the prefix) in the title-bar of the message-box window.
        /// </summary>
        /// <param name="caption">the text to use for the new title-bar content</param>
        /// <returns>a reference to this same object such that other method-calls may be chained</returns>
        public MessageBoxConfiguration SetCaptionAfterPrefix(string caption)
        {
            _captionAfterPrefix = caption;
            return this;
        }
        #endregion caption

        #region SummaryText
        /// <summary>
        /// Get or set the text that's shown in the upper, summary-text area.
        /// </summary>
        /// <remarks>
        /// Of the two major sections of text that are displayed within the message-box, this is the main summary of the message
        /// that is positioned above the detail-text.
        /// </remarks>
        public string SummaryText
        {
            get { return _summaryText; }
            set { _summaryText = value; }
        }

        /// <summary>
        /// Set the summary-text that's shown in the upper, main text area.
        /// </summary>
        /// <param name="summaryText">the string to use as the content of the summary-text section of the message-box</param>
        /// <returns>a reference to this same object such that other method-calls may be chained</returns>
        public MessageBoxConfiguration SetSummaryText(string summaryText)
        {
            _summaryText = summaryText;
            return this;
        }
       #endregion

        #region DetailText
        /// <summary>
        /// Get or set the detail-text to be shown to the user. This is the detailed description that is displayed under the summary,
        /// and is optional.
        /// </summary>
        /// <remarks>
        /// Of the two major sections of text that are displayed within the message-box, this is the more-detailed explaination
        /// of the message that is positioned underneath the summary-text. This is optional.
        /// </remarks>
        public string DetailText
        {
            get { return _detailText; }
            set { _detailText = value; }
        }

        /// <summary>
        /// Set the detail-text to be shown to the user.
        /// </summary>
        /// <param name="userText">the string to use as the content of the user-text section of the message-box</param>
        /// <returns>a reference to this same object such that other method-calls may be chained</returns>
        public MessageBoxConfiguration SetDetailText(string userText)
        {
            _detailText = userText;
            return this;
        }
        #endregion

        #region IsAsynchronous
        /// <summary>
        /// Get or set whether to show the message-box as a non-modal dialog window, and return immediately from the method-call that invoked it,
        /// as opposed to showing it as a modal window and waiting for it to close before returning. Default is false.
        /// </summary>
        public bool IsAsynchronous
        {
            get { return _isAsynchronous; }
            set { _isAsynchronous = value; }
        }

        /// <summary>
        /// Set whether to show the message-box as a non-modal dialog window, and return immediately from the method-call that invoked it,
        /// as opposed to showing it as a modal window and waiting for it to close before returning. Default is false.
        /// </summary>
        /// <param name="isToInvokeAsynchronously">true to display as a non-modal dialog (ie, asynchronously), false to operate synchronously</param>
        /// <returns>a reference to this same object such that other method-calls may be chained</returns>
        public MessageBoxConfiguration SetIsAsynchronous(bool isToInvokeAsynchronously)
        {
            IsAsynchronous = isToInvokeAsynchronously;
            return this;
        }
        #endregion

        #region IsToCenterOverParent
        /// <summary>
        /// Get or set whether we want to center the message-box window over that of the parent window. Default is true.
        /// </summary>
        public bool IsToCenterOverParent
        {
            get { return _isToCenterOverParent; }
            set { _isToCenterOverParent = value; }
        }

        /// <summary>
        /// Set whether we want to center the message-box window over that of the parent window. Default is true.
        /// </summary>
        /// <param name="isCentered"></param>
        /// <returns>a reference to this same object such that other method-calls may be chained</returns>
        public MessageBoxConfiguration SetToCenterOverParent(bool isToCenterOverParentWindow)
        {
            _isToCenterOverParent = isToCenterOverParentWindow;
            return this;
        }
        #endregion

        #region IsToBeTopmostWindow
        /// <summary>
        /// Get or set whether we want the message-box to position itself over top of any other window
        /// that is on the Windows Desktop. The default value is determined by
        /// MessageBoxDefaultConfiguration.IsTopmostWindowByDefault, which is false unless set.
        /// </summary>
        public bool? IsToBeTopmostWindow
        {
            get { return _isToBeTopmostWindow; }
            set { _isToBeTopmostWindow = value; }
        }

        /// <summary>
        /// Get whether the message-box is to be the topmost window - that is, to position itself over top of any other window,
        /// whether this has been explicitly set for this message-box or, if not, taking it's value from the default.
        /// </summary>
        public bool IsEffectivelyTopmostWindow
        {
            get
            {
                if (_isToBeTopmostWindow.HasValue)
                {
                    return _isToBeTopmostWindow.Value;
                }
                else
                {
                    return MessageBox.DefaultConfiguration.IsTopmostWindowByDefault;
                }
            }
        }

        /// <summary>
        /// Set whether we want the message-box to position itself over top of any other window
        /// that is on the Windows Desktop. The default value is determined by
        /// MessageBoxDefaultConfiguration.IsTopmostWindowByDefault, which is false unless set.
        /// </summary>
        /// <param name="isToBeOnTop">true if we want to position this over top of all other windows, null if we want it to simply take whatever the default is</param>
        /// <returns>a reference to this same object such that other method-calls may be chained</returns>
        public MessageBoxConfiguration SetToBeTopmostWindow(bool? isToBeOnTop)
        {
            _isToBeTopmostWindow = isToBeOnTop;
            return this;
        }
        #endregion

        #region MaximumTimeoutPeriodInSeconds
        /// <summary>
        /// Get or set the maximum value that you can set the TimeoutPeriodInSeconds property to,
        /// thus defining the uppermost-limit for how long this dialog-window will be shown
        /// before it takes a default response and closes. In units of seconds.
        /// This is currently fixed at One Hour.
        /// </summary>
        public static int MaximumTimeoutPeriodInSeconds
        {
            get { return _maximumTimeoutPeriodInSeconds; }
        }
        #endregion

        #region MessageType
        /// <summary>
        /// Get or set the basic type of this message-box (i.e., Information, Warning, Error, etc.).
        /// The default is None.
        /// </summary>
        public MessageBoxType MessageType
        {
            get { return _messageType; }
            set { _messageType = value; }
        }

        /// <summary>
        /// Set the basic type of this message-box (i.e., Information, Warning, Error, etc.). Default is None.
        /// </summary>
        /// <param name="messageBoxType">an enum-type that defines the basic purpose of this message-box</param>
        /// <returns>a reference to this same object such that other method-calls may be chained</returns>
        public MessageBoxConfiguration SetMessageType(MessageBoxType messageBoxType)
        {
            _messageType = messageBoxType;
            return this;
        }
        #endregion

        #region ParentElement
        /// <summary>
        /// Get or set the visual element that is considered to be the "parent" or owner of the message-box, such that it can know what
        /// to align itself with. This is null by default, which means no parent has been designated.
        /// </summary>
        public FrameworkElement ParentElement
        {
            get { return _parentElement; }
            set { _parentElement = value; }
        }

        /// <summary>
        /// Set the visual element that is considered to be the "parent" or owner of the message-box, such that it can know what
        /// to align itself with. This is null by default, which means no parent has been designated.
        /// </summary>
        /// <param name="parent">The parent-Window to associate the message-box with</param>
        /// <returns>a reference to this same object such that other method-calls may be chained</returns>
        public MessageBoxConfiguration SetParent(FrameworkElement parent)
        {
            this.ParentElement = parent;
            return this;
        }
        #endregion

        #region SignalThatMessageBoxHasEnded
        /// <summary>
        /// Raise the Completed event with parameters indicating the current message-box window and the user-response
        /// it was closed with.
        /// </summary>
        /// <remarks>
        /// This is attached to MessageBoxConfiguration because it's handler needs to exist at the instance-level,
        /// and the Configuration object is easily available to the developer.
        /// </remarks>
        /// <param name="messageBoxWindow">the message-box window that signaled this event</param>
        /// <param name="result">the user-response with which the message-box window was dismissed</param>
        public void SignalThatMessageBoxHasEnded(MessageBoxWindow messageBoxWindow, MessageResult result)
        {
            var args = new MessageBoxCompletedEventArgs(result, null, messageBoxWindow);
            this.Completed(this, args);
        }
        #endregion

        #region TimeoutPeriodInSeconds
        /// <summary>
        /// Get or set the time-span for which the MessageBox will be shown before it takes a default response and goes away. In seconds.
        /// Default is 10 seconds. Setting this to zero causes it to return to it's default value.
        /// </summary>
        public int TimeoutPeriodInSeconds
        {
            get { return _timeoutPeriodInSeconds; }
            set
            {
                _timeoutPeriodInSeconds = Math.Min(value, _maximumTimeoutPeriodInSeconds);
            }
        }

        /// <summary>
        /// Set the time-span for which the MessageBox will be shown before it takes a default response and goes away. In seconds.
        /// Default is 10 seconds.
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns>a reference to this same object such that other method-calls may be chained</returns>
        public MessageBoxConfiguration SetTimeoutPeriod(int seconds)
        {
            _timeoutPeriodInSeconds = Math.Min(seconds, _maximumTimeoutPeriodInSeconds);
            return this;
        }
        #endregion

        #region fields

        /// <summary>
        /// This specifies which of the pre-supplied buttons are to be shown. The default is just the Ok button.
        /// </summary>
        private MessageBoxButtons _buttonFlags = MessageBoxButtons.Ok;

        private string _buttonAbortText = "Abort";
        private string _buttonCancelText = "Cancel";
        private string _buttonCloseText = "Close";
        private string _buttonIgnoreText = "Ignore";
        private string _buttonNoText = "No";
        private string _buttonOkText = "Ok";
        private string _buttonRetryText = "Retry";
        private string _buttonYesText = "Yes";

        private string _buttonAbortToolTipText;
        private string _buttonCancelToolTipText;
        private string _buttonCloseToolTipText;
        private string _buttonIgnoreToolTipText;
        private string _buttonNoToolTipText;
        private string _buttonOkToolTipText;
        private string _buttonRetryToolTipText;
        private string _buttonYesToolTipText;

        /// <summary>
        /// The text that is shown in the message-box title-bar, after the prefix + ": "..
        /// The initial value for this is null.
        /// </summary>
        private string _captionAfterPrefix;
        /// <summary>
        /// The initial text to show in the title-bar of the message-box, to which a colon-space and then the caption-text is shown.
        /// This would normally be left null, so that the application's inherited facilities will generate this.
        /// </summary>
        private string _captionPrefix;
        /// <summary>
        /// Of the two major sections of text that are displayed within the message-box, this is the main summary of the message
        /// that is positioned above the detail-text.
        /// </summary>
        private string _summaryText;
        /// <summary>
        /// Of the two major sections of text that are displayed within the message-box, this is the more-detailed explaination
        /// of the message that is positioned underneath the summary-text. This is optional.
        /// </summary>
        private string _detailText;
        /// <summary>
        /// This flag indicates whether this message-box was invoked asynchronously;  ie with, for example, NotifyUserAsync
        /// </summary>
        private bool _isAsynchronous;
        /// <summary>
        /// This indicates whether we want to center the message-box window over that of the parent window. Default is true.
        /// </summary>
        private bool _isToCenterOverParent = true;
        /// <summary>
        /// This dictates whether we want the message-box to position itself over top of any other window
        /// that is on the Windows Desktop. Default is determined by MessageBoxDefaultConfiguration.IsTopmostWindowByDefault.
        /// </summary>
        private bool? _isToBeTopmostWindow;
        /// <summary>
        /// This number represents the greatist-possible timeout value that is allowed for a message-box.
        /// It's value is One Hour (3600 seconds).
        /// </summary>
        private const int _maximumTimeoutPeriodInSeconds = 3600;
        /// <summary>
        /// This denotes the basic "type" of this message-box - that is, whether it is a Warning, Error, Question, whatever.
        /// The default set in the constructor is None.
        /// </summary>
        private MessageBoxType _messageType;
        /// <summary>
        /// The visual element that is considered to be the "parent" or owner of the message-box, such that it can know what
        /// to align itself with. This is null by default, which means no parent has been designated.
        /// </summary>
        private FrameworkElement _parentElement;
        /// <summary>
        /// The time interval to wait before it "times-out" and we close the message-box automatically. Default is ten seconds.
        /// </summary>
        private int _timeoutPeriodInSeconds = 10;

        #endregion fields
    }
    #endregion class MessageBoxConfiguration
}

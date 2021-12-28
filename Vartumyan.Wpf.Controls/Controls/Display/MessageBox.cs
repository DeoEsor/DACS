#if PRE_4
#define PRE_5
#endif
using System;
using System.ComponentModel;
using System.Windows;


// Notes:
//   For simplicity I chose the name "DisplayBox", to be patterned after the ubiquitous "MessageBox"
//   upon which many of us have relied upon from C++ and Windows Forms,
//   and it does reflect our purpose better than "TaskDialog" which could connote any dialog for accomplishing a task
//   (a bit too generic).
//   msdn webpage describing task dialog design considerations:
//   http://msdn.microsoft.com/en-us/library/Aa511268#commitButtons


namespace Hurst.BaseLibWpf.Display
{
    #region types used with MessageBox
    /// <summary>
    /// This selects one of the predefined background bitmap images.
    /// </summary>
    public enum MessageBoxBackgroundTexturePreset
    {
        None,
        BlueTexture,
        BlueMarble,
        BrownMarble1,
        BrownMarble2,
        BrownTexture1,
        BrownTexture2,
        BrushedMetal,
        GrayTexture1,
        GrayTexture2,
        GrayMarble,
        GreenTexture1,
        GreenTexture2,
        GreenTexture3,
        GreenGradiant   // This one uses XAML gradiants instead of a bitmap
    }

    #region class MessageBoxCompletedEventArgs
    /// <summary>
    /// This class provides the argument for the Completed event's handler.
    /// </summary>
    public class MessageBoxCompletedEventArgs : AsyncCompletedEventArgs
    {
        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="result">the MessageResult that the message-box was ended with</param>
        /// <param name="error">an exception if it was thrown during the operation of the message-box, otherwise null</param>
        /// <param name="messageBoxWindow">the message-box that raised this event</param>
        public MessageBoxCompletedEventArgs(MessageResult result, Exception error, MessageBoxWindow messageBoxWindow)
#if PRE_4
            : base(error, false, messageBoxWindow)
#else
            : base(error: error, cancelled: false, userState: messageBoxWindow)
#endif
        {
            this.Result = result;
            this.MessageBoxWindow = messageBoxWindow;
            this.Configuration = messageBoxWindow._options;
        }

        /// <summary>
        /// Get the MessageBoxConfiguration that the message-box was invoked with.
        /// </summary>
        public MessageBoxConfiguration Configuration { get; private set; }

        /// <summary>
        /// Get the MessageResult that the message-box was ended with.
        /// </summary>
        public MessageResult Result { get; private set; }

        /// <summary>
        /// Get the MessageBoxWindow instance that the message-box was displayed with.
        /// </summary>
        public MessageBoxWindow MessageBoxWindow { get; private set; }
    }
    #endregion

    #endregion types used with MessageBox


    #region class MessageBox
    /// <summary>
    /// This is the MessageBox-manager class, the single-point-of-access for our message-box facility
    /// which displays notifications to the user via the MessageBoxWindow as opposed to the older MessageBox.
    /// </summary>
    public class MessageBox : IInterlocution, IDisposable
    {
        #region Constructor and factory instance methods
        /// <summary>
        /// Get a new instance of a MessageBox (which is an IInterlocution),
        /// given the text to use as a prefix for all captions.
        /// </summary>
        /// <param name="vendorName">the name of the business-entity that owns this application, which is included within the caption for all message-boxes</param>
        /// <param name="productName">the name of this application, which is included within the caption for all message-boxes</param>
        /// <returns>a new instance of MessageBox for use in your application</returns>
        public static MessageBox GetNewInstance(string vendorName, string productName)
        {
            if (StringLib.HasNothing(vendorName))
            {
                throw new ArgumentException("You must supply a value for vendorName - this is part of the mandatory title-bar prefix.");
            }
            if (StringLib.HasNothing(productName))
            {
                throw new ArgumentException("productName.");
            }
            var newMessageBoxManager = new MessageBox();
            newMessageBoxManager._captionPrefix = vendorName + " " + productName;
            return newMessageBoxManager;
        }

        public static MessageBox GetNewInstance(System.Windows.Application ownerApplication)
        {
            var newMessageBoxManager = new MessageBox();
            newMessageBoxManager._ownerApplication = ownerApplication;
            newMessageBoxManager._captionPrefix = newMessageBoxManager.GetAReasonableTitlebarPrefix();
            return newMessageBoxManager;
        }

        internal MessageBox()
        {
        }
        #endregion Constructor and factory instance methods

        #region AlertUser
        /// <summary>
        /// Static method to display a message-box to the user. Wait for his response or else close itself after the timeout has expired.
        /// This assumes only an Ok button, that it wants to be a top-most window and a timeout of five seconds.
        /// </summary>
        /// <param name="ofWhat">the text to show</param>
        /// <param name="messageType">which basic type of message this is (optional - defaults to Information)</param>
        /// <param name="captionAfterPrefix">what to show in the titlebar of this message-box, after the standard prefix (optional)</param>
        /// <param name="timeout">the maximum time to show it, in seconds (optional - the default is 5)</param>
        /// <param name="parent">the visual-element to consider as the parent, or owner, of this message-box (optional)</param>
        public static void AlertUser(string ofWhat,
#if !PRE_4
                                     MessageBoxType messageType = MessageBoxType.Information,
                                     string captionAfterPrefix = null,
                                     int timeout = 5,  // Assume five seconds for the timeout value unless it is explicitly specified.
                                     Object parent = null)
        {
            MessageBox.GetNewInstance(Application.Current).NotifyUser(summaryText: ofWhat,
                                                                      detailText: null,
                                                                      buttons: MessageBoxButtons.Ok,
                                                                      messageType: messageType,
                                                                      captionAfterPrefix: captionAfterPrefix,
                                                                      isTopmostWindow: DefaultConfiguration.IsTopmostWindowByDefault,
                                                                      timeout: timeout,
                                                                      parent: (FrameworkElement)parent);
#else
                                     MessageBoxType messageType,
                                     string captionAfterPrefix,
                                     int timeout,
                                     FrameworkElement parent)
        {
            MessageBox.GetNewInstance(Application.Current).NotifyUser(ofWhat,
                                                                      null,
                                                                      MessageBoxButtons.Ok,
                                                                      messageType,
                                                                      captionAfterPrefix,
                                                                      DefaultConfiguration.IsTopmostWindowByDefault,
                                                                      timeout,
                                                                      parent);
#endif
        }
        #endregion AlertUser

        #region the NotifyUser.. methods

        #region NotifyUser
        /// <summary>
        /// Display a message-box to the user, based upon the given options.
        /// This particular method is the one that all of the other Notify methods invoke to do the actual work.
        /// </summary>
        /// <param name="options">a MessageBoxConfiguration object that fully specifies everything about the message-box</param>
        /// <returns>a MessageResult indicating which action the user took, or TimedOut if the user took no action before the timeout expired</returns>
        public MessageResult NotifyUser(IMessageBoxConfiguration options)
        {
            MessageBoxConfiguration configuration = options as MessageBoxConfiguration;

            var messageBoxWindow = new MessageBoxWindow(this, configuration);

            // If the developer who is calling this has provided a custom visual element
            // to show within the message-box, set that now.
            //CBL
            //if (_customElement != null)
            //{
            //    messageBoxWindow.SetCustomElement(_customElement);
            //}

            // If we are doing testing, and have specified that a specific button is to be emulated as having been selected by the user,
            // then verify that that button is indeed present..
            if (IsTesting)
            {
                if (TestFacility.ButtonResultToSelect.HasValue)
                {
                    string complaint;
                    if (!GetWhetherButtonIsIncluded(configuration.ButtonFlags, out complaint))
                    {
                        // Announce the error  
                        throw new ArgumentOutOfRangeException(complaint);
                    }
                }
            }

            //TODO: The following should not be needed for Silverlight..
            // Try to give it a parent-window to position itself relative to.
#if !SILVERLIGHT
            FrameworkElement parentElement = configuration.ParentElement;
            Window parentWindow = null;
            if (parentElement == null)
            {
                // When running unit-tests, Application.Current might be null.
                if (Application.Current != null)
                {
                    parentWindow = Application.Current.MainWindow;
                    // If the parent-window has not actually been shown yet, this would cause an exception.
                    if (parentWindow != null && parentWindow.IsLoaded)
                    {
                        messageBoxWindow.Owner = parentWindow;
                    }
                }
            }
            else if (parentElement.IsLoaded)
            {
                parentWindow = parentElement as Window;
                if (parentWindow != null)
                {
                    messageBoxWindow.Owner = parentWindow;
                }
            }
#endif

            // Finally, show the message-box.
            //TODO: for Silverlight, need to prepare to receive the result asynchronously!!!
#if SILVERLIGHT
            messageBox.Show();
#else
            _messageBoxWindow = messageBoxWindow;
            if (configuration.IsAsynchronous)
            {
                messageBoxWindow.Show();
            }
            else
            {
                messageBoxWindow.ShowDialog();
            }
#endif
            return messageBoxWindow.Result;
        }

        public MessageResult WarnUser(string ofWhat)
        {
            return WarnUser(ofWhat, null);
        }

        public MessageResult WarnUser(string ofWhat, Object parent)
        {
            // Use the default options for everything except that which is explicitly set for this instance.
            var options = new MessageBoxConfiguration(this.Configuration);
            options.SummaryText = ofWhat;
            options.DetailText = null;
            options.CaptionAfterPrefix = "Warning:";
            options.ButtonFlags = MessageBoxButtons.Ok;
            options.MessageType = MessageBoxType.Warning;
            options.IsToBeTopmostWindow = DefaultConfiguration.IsTopmostWindowByDefault;
#if !SILVERLIGHT
            options.ParentElement = (FrameworkElement)parent;
#endif
            options.TimeoutPeriodInSeconds = 0;  // Assume zero for the timeout value, which invokes the default value.
            return NotifyUser(options);
        }

        /// <summary>
        /// Display a message-box to the user. Wait for his response or else close itself after the timeout has expired.
        /// The message-type is assumed to be MessageBoxType.Information
        /// </summary>
        /// <param name="ofWhat">The text message to display to the user</param>
        /// <returns>A MessageResult that indicates the user's response</returns>
        public MessageResult NotifyUser(string ofWhat)
        {
            return NotifyUser(ofWhat, null);
        }

        /// <summary>
        /// Display a message-box to the user. Wait for his response or else close itself after the timeout has expired.
        /// The message-type is assumed to be MessageBoxType.Information
        /// </summary>
        /// <param name="ofWhat">The text message to display to the user</param>
        /// <param name="parent">the visual-element to consider as the parent, or owner, of this message-box (optional)</param>
        /// <returns>A MessageResult that indicates the user's response</returns>
        public MessageResult NotifyUser(string ofWhat, Object parent)
        {
            // Use the default options for everything except that which is explicitly set for this instance.
            var options = new MessageBoxConfiguration(this.Configuration);
            options.SummaryText = ofWhat;
            options.DetailText = null;
            options.CaptionAfterPrefix = null;
            options.ButtonFlags = MessageBoxButtons.Ok;
            options.MessageType = MessageBoxType.Information;
            options.IsToBeTopmostWindow = DefaultConfiguration.IsTopmostWindowByDefault;
#if !SILVERLIGHT
            options.ParentElement = (FrameworkElement)parent;
#endif
            options.TimeoutPeriodInSeconds = 0;  // Assume zero for the timeout value, which invokes the default value.
            return NotifyUser(options);
        }

        /// <summary>
        /// Display a message-box to the user. Wait for his response or else close itself after the timeout has expired.
        /// The message-type is assumed to be MessageBoxType.Information
        /// </summary>
        /// <param name="summaryText">the summary text to show in the upper area</param>
        /// <param name="detailText">the detail text to show in the lower area</param>
        /// <param name="captionAfterPrefix">what to show in the titlebar of this message-box, after the standard prefix (set to null to accept default)</param>
        /// <param name="parent">the visual-element to consider as the parent, or owner, of this message-box (optional)</param>
        /// <returns>a MessageResult that signals what the user clicked, or whether it timed-out</returns>
#if !PRE_4
        public MessageResult NotifyUser(string summaryText, string detailText, string captionAfterPrefix, Object parent = null)
#else
        public MessageResult NotifyUser(string summaryText, string detailText, string captionAfterPrefix, Object parent)
#endif
        {
            // Use the default options for everything except that which is explicitly set for this instance.
            var options = new MessageBoxConfiguration(this.Configuration);
            options.SummaryText = summaryText;
            options.DetailText = detailText;
            options.CaptionAfterPrefix = captionAfterPrefix;
            options.ButtonFlags = MessageBoxButtons.Ok;
            options.MessageType = MessageBoxType.Information;
            options.IsToBeTopmostWindow = DefaultConfiguration.IsTopmostWindowByDefault;
#if !SILVERLIGHT
            options.ParentElement = (FrameworkElement)parent;
#endif
            options.TimeoutPeriodInSeconds = 0;  // Assume zero for the timeout value, which invokes the default value.
            return NotifyUser(options);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="summaryText"></param>
        /// <param name="detailText"></param>
        /// <param name="captionAfterPrefix">provide null for this parameter if you want the system to automatically generate a suitable caption</param>
        /// <param name="parent"></param>
        /// <returns></returns>
#if !PRE_4
        public MessageResult WarnUser(string summaryText, string detailText, string captionAfterPrefix, Object parent = null)
#else
        public MessageResult WarnUser(string summaryText, string detailText, string captionAfterPrefix, Object parent)
#endif
        {
            // Use the default options for everything except that which is explicitly set for this instance.
            var options = new MessageBoxConfiguration(this.Configuration);
            options.SummaryText = summaryText;
            options.DetailText = detailText;
            options.CaptionAfterPrefix = captionAfterPrefix;
            options.ButtonFlags = MessageBoxButtons.Ok;
            options.MessageType = MessageBoxType.Warning;
            options.IsToBeTopmostWindow = DefaultConfiguration.IsTopmostWindowByDefault;
#if !SILVERLIGHT
            options.ParentElement = (FrameworkElement)parent;
#endif
            options.TimeoutPeriodInSeconds = 0;  // Assume zero for the timeout value, which invokes the default value.
            return NotifyUser(options);
        }

        /// <summary>
        /// Display a message-box to the user. Wait for his response or else close itself after the timeout has expired.
        /// This is the overload that has all of the options, which the other methods call.
        /// </summary>
        /// <param name="summaryText">the summary text to show in the upper area</param>
        /// <param name="detailText">the detail text to show in the lower area</param>
        /// <param name="buttons">which buttons to show</param>
        /// <param name="messageType">which basic type of message this is</param>
        /// <param name="captionAfterPrefix">what to show in the titlebar of this message-box, after the standard prefix (may be null)</param>
        /// <param name="isTopmostWindow">whether to make the message-box the top-most window on the user's desktop (may be null to just accept the default)</param>
        /// <param name="timeout">the maximum time to show it, in seconds (make this zero to accept the default)</param>
        /// <param name="parent">the visual-element to consider as the parent, or owner, of this message-box (may be null)</param>
        /// <returns>a MessageResult indicating which action the user took, or TimedOut if the user took no action before the timeout expired</returns>
        public MessageResult NotifyUser(string summaryText,
                                             string detailText,
                                             MessageBoxButtons buttons,
                                             MessageBoxType messageType,
                                             string captionAfterPrefix,
                                             bool? isTopmostWindow,
                                             int timeout,  // Assume zero for the timeout value, which invokes the default value.
                                             Object parent)
        {
            // Use the default options for everything except that which is explicitly set for this instance.
            var options = new MessageBoxConfiguration(this.Configuration);
            options.SummaryText = summaryText;
            options.DetailText = detailText;
            options.CaptionAfterPrefix = captionAfterPrefix;
            options.ButtonFlags = buttons;
            options.MessageType = messageType;
            if (isTopmostWindow.HasValue)
            {
                options.IsToBeTopmostWindow = isTopmostWindow.Value;
            }
            else
            {
                options.IsToBeTopmostWindow = DefaultConfiguration.IsTopmostWindowByDefault;
            }
#if !SILVERLIGHT
            options.ParentElement = (FrameworkElement)parent;
#endif
            options.TimeoutPeriodInSeconds = timeout;
            return NotifyUser(options);
        }

        #region NotifyUserAsync
#if !PRE_4  // If .NET 3.5 or earlier, don't bother with this.

        /// <summary>
        /// Display a message-box to the user as a non-modal dialog window, and return immediately.
        /// The message-type is assumed to be MessageBoxType.Information
        /// </summary>
        /// <param name="summaryText">The text message to display to the user</param>
        /// <param name="parent">(optional) the visual-element to consider as the parent, or owner, of this message-box (optional)</param>
        public void NotifyUserAsync(string summaryText, Object parent = null)
        {
            var options = new MessageBoxConfiguration(this.Configuration)
                .SetMessageType(MessageBoxType.Information)
                .SetIsAsynchronous(true)
                .SetSummaryText(summaryText)
                .SetButtonFlags(MessageBoxButtons.Ok)
                .SetTimeoutPeriod(0)
                .SetParent((FrameworkElement)parent);
            NotifyUser(options);
        }

        /// <summary>
        /// Display a message-box to the user as a non-modal dialog window, and return immediately.
        /// The message-type is assumed to be MessageBoxType.Information
        /// </summary>
        /// <param name="summaryText">the summary text to show in the upper area</param>
        /// <param name="detailText">the detail text to show in the lower area</param>
        /// <param name="captionAfterPrefix">what to show in the titlebar of this message-box, after the standard prefix (set to null to accept default)</param>
        /// <param name="parent">the visual-element to consider as the parent, or owner, of this message-box (optional)</param>
        public void NotifyUserAsync(string summaryText, string detailText, string captionAfterPrefix, Object parent = null)
        {
            var options = new MessageBoxConfiguration(this.Configuration)
                .SetButtonFlags(MessageBoxButtons.Ok)
                .SetCaptionAfterPrefix(captionAfterPrefix)
                .SetDetailText(detailText)
                .SetSummaryText(summaryText)
                .SetIsAsynchronous(true)
                .SetMessageType(MessageBoxType.Information)
                .SetParent((FrameworkElement)parent)
                .SetTimeoutPeriod(0);
            NotifyUser(options);
        }

        /// <summary>
        /// Display a message-box to the user. Wait for his response or else close itself after the timeout has expired.
        /// This is the overload that has all of the options, which the other methods call.
        /// </summary>
        /// <param name="summaryText">the summary text to show in the upper area</param>
        /// <param name="detailText">the detail text to show in the lower area</param>
        /// <param name="buttons">which buttons to show</param>
        /// <param name="messageType">the basic type of message-box to show (optional - default is Information)</param>
        /// <param name="captionAfterPrefix">what to show in the titlebar of this message-box, after the standard prefix (optional)</param>
        /// <param name="isTopmostWindow">whether to force this message-box to be over top of all other windows (optional - default is null which means DefaultConfiguration.IsTopmostByDefault dictates)</param>
        /// <param name="timeoutInSeconds">the maximum time to show it, in seconds (optional)</param>
        /// <param name="parent">the visual-element to consider as the parent, or owner, of this message-box (optional)</param>
        /// <returns>a MessageResult indicating which action the user took, or TimedOut if the user took no action before the timeout expired</returns>
        public void NotifyUserAsync(string summaryText,
                                    string detailText,
                                    MessageBoxButtons buttons,
                                    MessageBoxType messageType = MessageBoxType.Information,
                                    string captionAfterPrefix = null,
                                    bool? isTopmostWindow = null,
                                    int timeoutInSeconds = 0,  // Assume zero for the timeout value, which invokes the default value.
                                    Object parent = null)
        {
            // Use the default options for everything except that which is explicitly set for this instance.
            var options = new MessageBoxConfiguration(this.Configuration)
                .SetButtonFlags(buttons)
                .SetCaptionAfterPrefix(captionAfterPrefix)
                .SetDetailText(detailText)
                .SetSummaryText(summaryText)
                .SetIsAsynchronous(true)
                .SetMessageType(messageType)
                .SetTimeoutPeriod(timeoutInSeconds)
                .SetParent((FrameworkElement)parent);

            if (isTopmostWindow.HasValue)
            {
                options.SetToBeTopmostWindow(isTopmostWindow.Value);
            }
            else
            {
                options.SetToBeTopmostWindow(MessageBox.DefaultConfiguration.IsTopmostWindowByDefault);
            }
            NotifyUser(options);
        }
#endif
        #endregion NotifyUserAsync

        #endregion NotifyUser

        #region NotifyUserOfMistake
        /// <summary>
        /// Show a message-box to notify the user of a mistake that he has made in his operation of this program.
        /// </summary>
        /// <param name="summaryText">the text that goes in the upper, basic-message area</param>
        /// <param name="detailText">the more detailed text that goes in the lower area</param>
        /// <param name="captionAfterPrefix">what to put in the title-bar of the message-box - after the prefix that would contain the vendor and program-name</param>
        /// <param name="parent">the user-interface element to serve as the parent of the message-box, so that it can center itself over that</param>
        /// <returns>a MessageResult indicating what the user clicked on to close the message-box</returns>
#if !PRE_4
        public MessageResult NotifyUserOfMistake(string summaryText, string detailText = null, string captionAfterPrefix = null, Object parent = null)
#else
        public MessageResult NotifyUserOfMistake(string summaryText, string detailText, string captionAfterPrefix, Object parent)
#endif
        {
            //TODO: May want to provide for the possibility of having other buttons for this type of message-box. ?
            // Use the default options for everything except that which is explicitly set for this instance.
            var options = new MessageBoxConfiguration(this.Configuration);
            // Derive a timeout value..
            int timeout = 0;
            if (_defaultConfiguration != null)
            {
                timeout = DefaultConfiguration.GetDefaultTimeoutValueFor(MessageBoxType.UserMistake);
            }
            if (timeout == 0)
            {
                timeout = DefaultConfiguration.GetDefaultTimeoutValueFor(MessageBoxType.UserMistake);
            }
            // Derive a suitable text to use for the message-box caption if none was specified..
            string whatToUseForCaptionAfterPrefix = captionAfterPrefix;
            if (StringLib.HasNothing(whatToUseForCaptionAfterPrefix))
            {
                if (DefaultConfiguration.DefaultCaptionForUserMistakes != null)
                {
                    whatToUseForCaptionAfterPrefix = DefaultConfiguration.DefaultCaptionForUserMistakes;
                }
                else
                {
                    //TODO: May want to be a little more creative here.
                    whatToUseForCaptionAfterPrefix = "Oops!";
                }
            }
#if SILVERLIGHT
            return NotifyUser(summaryText, detailText, whatToUseForCaptionAfterPrefix, MessageBoxButtons.Ok, MessageBoxType.UserMistake, timeout);
#else
            options.SummaryText = summaryText;
            options.DetailText = detailText;
            options.CaptionAfterPrefix = whatToUseForCaptionAfterPrefix;
            options.ButtonFlags = MessageBoxButtons.Ok;
            options.MessageType = MessageBoxType.UserMistake;
            options.ParentElement = (FrameworkElement)parent;
            options.TimeoutPeriodInSeconds = timeout;
            return NotifyUser(options);
#endif
        }
        #endregion NotifyUserOfMistake

        #region NotifyUserOfMistakeAsync
#if !PRE_4
        /// <summary>
        /// Asynchronously show a message-box to notify the user of a mistake that he has made in his operation of this program, and return immediately.
        /// </summary>
        /// <param name="summaryText">the text that goes in the upper, basic-message area</param>
        /// <param name="detailText">the more detailed text that goes in the lower area</param>
        /// <param name="captionAfterPrefix">what to put in the title-bar of the message-box - after the prefix that would contain the vendor and program-name</param>
        /// <param name="parent">the user-interface element to serve as the parent of the message-box, so that it can center itself over that</param>
        public void NotifyUserOfMistakeAsync(string summaryText, string detailText, string captionAfterPrefix, FrameworkElement parent = null)
        {
            //TODO:
            // Need to ensure it is asynchronous,
            // that we have a means of responding to the user-selection,
            // that the parent-window doesn't rise to occlude this one, since it will be shown non-modally
            // I'm not even sure this facility is even needed.
            NotifyUserOfMistake(summaryText, detailText, captionAfterPrefix, parent);
        }
#endif
        #endregion NotifyUserOfMistakeAsync

        #region NotifyUserOfError

        /// <summary>
        /// This is the main base-method that other overloads should call.
        /// This can be called from a background thread.
        /// </summary>
        /// <param name="toUser">the main message directed at the end-user of the application</param>
        /// <param name="toDeveloper">supplemental text provided as help for the software developer</param>
        /// <param name="captionAfterPrefix">what to put into the title-bar of the message-box</param>
        /// <param name="exception">the Exception to describe, if non-null</param>
        /// <param name="parent">the parent-window which owns this message-box</param>
#if !PRE_4
        public void NotifyUserOfError(string toUser, string toDeveloper, string captionAfterPrefix, Exception exception, int timeout = 0, Object parent = null)
#else
        public void NotifyUserOfError(string toUser, string toDeveloper, string captionAfterPrefix, Exception exception, int timeout, Object parent)
#endif
        {
            // Ensure the code is called from the application's GUI thread.
            //TODO: Commented out for SL. In the case of SL, do I need to provide for calling this asynchronously?
#if !SILVERLIGHT
            Application.Current.InvokeIfRequired(() =>
            {
#endif
                if (StringLib.HasNothing(toUser))
                {
                    toUser = DefaultConfiguration.DefaultSummaryTextForErrors;
                }
                string detailText = toDeveloper;
                if (StringLib.HasNothing(detailText))
                {
                    if (exception != null)
                    {
                        detailText = exception.Message;
                    }
                }
                if (StringLib.HasNothing(captionAfterPrefix))
                {
                    captionAfterPrefix = GetExpressionOfDismay();
                }
                //CBL  Should note within the documentation that, if the developer implements an IInterlocution
                //      that invokes both this MessageBox as well as a logger, that the console-output of the logger
                //      should be disabled if he wants to avoid duplicate console output.
                if (exception == null)
                {
                    Console.Write("NotifyUserOfError(toUser: " + toUser);
                    if (StringLib.HasSomething(toDeveloper))
                    {
                        Console.WriteLine(", toDeveloper: " + toDeveloper);
                    }
                    else
                    {
                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.Write("NotifyUserOfError(toUser: " + toUser + ", toDeveloper: " + toDeveloper + Environment.NewLine + ",  exception: ");
                    Console.WriteLine(exception.ToString());
                }
#if SILVERLIGHT
                NotifyUser(toUser, detailText, captionAfterPrefix, MessageBoxButtons.Ok, MessageBoxType.Error, timeout);
#else
#if !PRE_4
                NotifyUser(summaryText: toUser,
                           detailText: detailText,
                           buttons: MessageBoxButtons.Ok,
                           messageType: MessageBoxType.Error,
                           captionAfterPrefix: captionAfterPrefix,
                           isTopmostWindow: null,
                           timeout: timeout,
                           parent: parent);
#else
                NotifyUser(toUser,
                           detailText,
                           MessageBoxButtons.Ok,
                           MessageBoxType.Error,
                           captionAfterPrefix,
                           null,
                           timeout,
                           parent);
#endif
#endif

#if !SILVERLIGHT
            });
#endif
        }

#if !PRE_4
        public void NotifyUserOfError(string toUser, string toDeveloper = null, Object parent = null)
        {
            NotifyUserOfError(toUser: toUser, toDeveloper: toDeveloper, captionAfterPrefix: null, exception: null, parent: parent);
        }
#else
        public void NotifyUserOfError(string toUser, string toDeveloper, Object parent)
        {
            NotifyUserOfError(toUser, toDeveloper, null, null, 0, parent);
        }
#endif

#if !PRE_4
        public void NotifyUserOfError(string toUser, string toDeveloper, Exception exception, Object parent = null)
        {
            NotifyUserOfError(toUser: toUser, toDeveloper: toDeveloper, captionAfterPrefix: null, exception: exception, parent: parent);
        }
#else
        public void NotifyUserOfError(string toUser, string toDeveloper, Exception exception, Object parent)
        {
            NotifyUserOfError(toUser, toDeveloper, null, exception, 0, parent);
        }
#endif

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="parent"></param>
        public void NotifyUserOfError(Exception exception, Object parent)
        {
#if !PRE_4
            NotifyUserOfError(toUser: null,
                              toDeveloper: null,
                              captionAfterPrefix: null,
                              exception: exception,
                              timeout: 0,
                              parent: parent);
#else
            NotifyUserOfError(null,
                              null,
                              null,
                              exception,
                              0,
                              parent);
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        public void NotifyUserOfError(Exception exception)
        {
#if !PRE_4
            NotifyUserOfError(toUser: null,
                              toDeveloper: null,
                              captionAfterPrefix: null,
                              exception: exception,
                              timeout: 0,
                              parent: null);
#else
            NotifyUserOfError(null,
                              null,
                              null,
                              exception,
                              0,
                              null);
#endif
        }
        #endregion NotifyUserOfError

        #region AskYesOrNo
        /// <summary>
        /// Display a message-box to the user asking a Yes-or-No question. Wait for his response or else close itself after the timeout has expired.
        /// </summary>
        /// <param name="question">the text of the question to pose to the user, which will go into the summary-text area of the message-box</param>
        /// <param name="defaultAnswer">this is the user-response to assume if the message-box is closed via its system-menu or times-out</param>
        /// <param name="detailText">additional text that can go into the detail-text area (optional)</param>
        /// <param name="caption">text to add to the normal default title-bar prefix that will appear as the 'caption' for this message-box (optional)</param>
        /// <param name="parent">the visual-element to consider as the parent, or owner, of this message-box (optional)</param>
        /// <returns>a MessageResult indicating which action the user took, or TimedOut if the user took no action before the timeout expired</returns>
#if !PRE_4
        public MessageResult AskYesOrNo(string question,
                                        MessageResult defaultAnswer,
                                        string detailText = null,
                                        string caption = null,
                                        Object parent = null)
#else
        public MessageResult AskYesOrNo(string question,
                                        MessageResult defaultAnswer,
                                        string detailText,
                                        string caption,
                                        Object parent)
#endif
        {
#if SILVERLIGHT
            return NotifyUser(question, null, sFullCaption, MessageBoxButtons.Yes | MessageBoxButtons.No, MessageBoxType.Question, timeout);
#else
#if !PRE_4
            MessageResult r = NotifyUser(summaryText: question,
                                         detailText: null,
                                         buttons: MessageBoxButtons.Yes | MessageBoxButtons.No,
                                         messageType: MessageBoxType.Question,
                                         captionAfterPrefix: caption,
                                         isTopmostWindow: null,
                                         timeout: 0,
                                         parent: parent);
#else
            MessageResult r = NotifyUser(question,
                                         null,
                                         MessageBoxButtons.Yes | MessageBoxButtons.No,
                                         MessageBoxType.Question,
                                         caption,
                                         null,
                                         0,
                                         parent);
#endif
            if (r == MessageResult.TimedOut)
            {
                return defaultAnswer;
            }
            else
            {
                return r;
            }
#endif
        }
        #endregion

        #region WarnUserAndAskYesOrNo
        /// <summary>
        /// Display a message-box to the user asking a Yes-or-No question. Wait for his response or else close itself after the timeout has expired.
        /// This is the same as AskYesOrNo, except that it presents it as a warning.
        /// </summary>
        /// <param name="question">the text of the question to pose to the user, which will go into the summary-text area of the message-box</param>
        /// <param name="defaultAnswer">this is the user-response to assume if the message-box is closed via its system-menu or times-out</param>
        /// <param name="detailText">additional text that can go into the detail-text area (optional)</param>
        /// <param name="captionAfterPrefix">text to add to the normal default title-bar prefix that will appear as the 'caption' for this message-box (optional)</param>
        /// <param name="parent">the visual-element to consider as the parent, or owner, of this message-box (optional)</param>
        /// <returns>a MessageResult indicating which action the user took, or TimedOut if the user took no action before the timeout expired</returns>
#if !PRE_4
        public MessageResult WarnUserAndAskYesOrNo(string question,
                                                    MessageResult defaultAnswer,
                                                    string detailText = null,
                                                    string captionAfterPrefix = null,
                                                    Object parent = null)
#else
        public MessageResult WarnUserAndAskYesOrNo(string question,
                                                    MessageResult defaultAnswer,
                                                    string detailText,
                                                    string captionAfterPrefix,
                                                    Object parent)
#endif
        {
#if SILVERLIGHT
            return NotifyUser(question, null, sFullCaption, MessageBoxButtons.Yes | MessageBoxButtons.No, MessageBoxType.Question, timeout);
#else
            string captionSuffix = captionAfterPrefix ?? "Warning:";
            //TODO: This is identical to the AskYesOrNo method. I need a different icon for this.
#if !PRE_4
            MessageResult r = NotifyUser(summaryText: question,
                                         detailText: null,
                                         buttons: MessageBoxButtons.Yes | MessageBoxButtons.No,
                                         messageType: MessageBoxType.Question,
                                         captionAfterPrefix: captionSuffix,
                                         isTopmostWindow: null,
                                         timeout: 0,
                                         parent: parent);
#else
            MessageResult r = NotifyUser(question,
                                         null,
                                         MessageBoxButtons.Yes | MessageBoxButtons.No,
                                         MessageBoxType.Question,
                                         captionSuffix,
                                         null,
                                         0,
                                         parent);
#endif
            if (r == MessageResult.TimedOut)
            {
                return defaultAnswer;
            }
            else
            {
                return r;
            }
#endif
        }
        #endregion WarnAndAskYesOrNo

        #region Show methods, for backward compatibility with MessageBox

        // These Show methods are simply provided for backward compatibility.
        //TODO: What should this really be? and it's duplicated in MessageBoxWindow.
        private const MessageResult _defaultResult = MessageResult.TimedOut;

        public MessageResult Show(string messageBoxText)
        {
            string caption = _captionPrefix;
            return Show(messageBoxText, caption, MessageBoxButtons.Ok, MessageBoxType.None, _defaultResult);
        }

        public MessageResult Show(string messageBoxText, string caption)
        {
            return Show(messageBoxText, caption, MessageBoxButtons.Ok, MessageBoxType.None, _defaultResult);
        }

        public MessageResult Show(string messageBoxText, string caption, MessageBoxButtons buttons)
        {
            return Show(messageBoxText, caption, buttons, MessageBoxType.None, _defaultResult);
        }

        public MessageResult Show(string messageBoxText, string caption, MessageBoxButtons buttons, MessageBoxType messageType)
        {
            return Show(messageBoxText, caption, buttons, messageType, _defaultResult);
        }

        public MessageResult Show(string messageBoxText, string caption, MessageBoxButtons buttons, MessageBoxType messageType, MessageResult defaultResult)
        {
#if !SILVERLIGHT
            FrameworkElement owner = Application.Current.MainWindow;
            return Show(messageBoxText, caption, buttons, messageType, _defaultResult, owner);
#else
            return Show(messageBoxText, caption, buttons, messageType, _defaultResult);
#endif
        }

        public MessageResult Show(string messageBoxText, FrameworkElement owner)
        {
            string caption = GetAReasonableTitlebarPrefix();
            return Show(messageBoxText, caption, MessageBoxButtons.Ok, MessageBoxType.None, _defaultResult, owner);
        }

        public MessageResult Show(string messageBoxText, string caption, FrameworkElement owner)
        {
            return Show(messageBoxText, caption, MessageBoxButtons.Ok, MessageBoxType.None, _defaultResult, owner);
        }

        public MessageResult Show(string messageBoxText, string caption, MessageBoxButtons buttons, FrameworkElement owner)
        {
            return Show(messageBoxText, caption, buttons, MessageBoxType.None, _defaultResult, owner);
        }

        public MessageResult Show(string messageBoxText, string caption, MessageBoxButtons buttons, MessageBoxType messageType, MessageResult defaultResult, FrameworkElement owner)
        {

            int defaultTimeout = DefaultConfiguration.GetDefaultTimeoutValueFor(messageType);
#if !SILVERLIGHT
#if !PRE_4
            MessageResult r = NotifyUser(summaryText: messageBoxText,
                                         detailText: "",
                                         buttons: buttons,
                                         messageType: messageType,
                                         captionAfterPrefix: caption,
                                         isTopmostWindow: null,
                                         timeout: defaultTimeout,
                                         parent: owner);
#else
            MessageResult r = NotifyUser(messageBoxText,
                                         "",
                                         buttons,
                                         messageType,
                                         caption,
                                         null,
                                         defaultTimeout,
                                         owner);
#endif
#else
            MessageResult r = NotifyUser(messageBoxText, "", caption, buttons, messageType, defaultTimeout);
#endif
            // In this library, we ordinarily consider the TimedOut result to be the default result,
            // but here the caller of this method has specified a specific defaultResult.
            if (r == MessageResult.TimedOut)
            {
                r = defaultResult;
            }
            return r;
        }

        #endregion Show methods

        #endregion the NotifyUser.. methods

        #region CaptionPrefix
        /// <summary>
        /// Get or set the text that's shown as the prefix of the title-bar of the Window.
        /// </summary>
        public string CaptionPrefix
        {
            get { return _captionPrefix; }
            set { _captionPrefix = value; }
        }

        /// <summary>
        /// Set the text that is to be shown as the prefix for the title-bar text of the message-box window.
        /// </summary>
        /// <param name="captionPrefix">the text to use for the title-bar prefix</param>
        /// <returns>a reference to this same object such that other method-calls may be chained</returns>
        public MessageBox SetCaptionPrefix(string captionPrefix)
        {
            _captionPrefix = captionPrefix;
            return this;
        }
        #endregion

        #region Configuration
        /// <summary>
        /// Get or set the MessageBoxConfiguration to use for message-boxes.
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
            set { _configuration = value; }
        }

        /// <summary>
        /// Set the options to use for message-boxes.
        /// </summary>
        /// <param name="options">The MessageBoxConfiguration that contains the options you want to set (the MessageType property is ignored)</param>
        public void SetConfiguration(MessageBoxConfiguration options)
        {
            this.Configuration = options;
        }
        #endregion

        #region DefaultConfiguration
        /// <summary>
        /// Get or set the MessageBoxDefaultConfiguration to use by default for future invocations of a message-box.
        /// </summary>
        public static MessageBoxDefaultConfiguration DefaultConfiguration
        {
            get
            {
                if (_defaultConfiguration == null)
                {
                    _defaultConfiguration = new MessageBoxDefaultConfiguration();
                }
                return _defaultConfiguration;
            }
            set { _defaultConfiguration = value; }
        }

        /// <summary>
        /// Set the options to use by default for subsequent invocations of a message-box.
        /// </summary>
        /// <param name="defaultConfiguration">The default options to set (the MessageType property is ignored)</param>
        public static void SetDefaultConfiguration(MessageBoxDefaultConfiguration defaultConfiguration)
        {
            DefaultConfiguration = defaultConfiguration;
        }
        #endregion

        #region unit-testing facilities

        #region BeginTest
        /// <summary>
        /// Turn on test-mode, and get a new instance of MessageBox.
        /// </summary>
        /// <returns></returns>
        public static MessageBox BeginTest()
        {
            var newInstance = GetNewInstance("TestVendor", "TestApp");
            // Having _testFacility being non-null puts this class into test-mode.
            newInstance._testFacility = new MessageBoxTestFacility(newInstance);
            return newInstance;
        }
        #endregion

        #region IsTesting
        /// <summary>
        /// Get whether message-boxes are being used in automated-test mode.
        /// This provides a way to check IsTesting without instanciating a MessageBoxTestFacility object.
        /// </summary>
        public bool IsTesting
        {
            get
            {
                if (_testFacility == null)
                {
                    return false;
                }
                else
                {
                    return TestFacility.IsTesting;
                }
            }
        }
        #endregion

        public MessageBoxWindow MessageBoxWindow
        {
            get { return _messageBoxWindow; }
        }

        #region TestFacility
        /// <summary>
        /// Get the message-box test-options that apply when running automated tests.
        /// </summary>
        public MessageBoxTestFacility TestFacility
        {
            get
            {
                return _testFacility;
            }
        }
        #endregion

        #region TextOf(the various buttons)

        public static string TextOfCancelButton
        {
            //TODO: This is hard-coded just for now -- it really needs to be converted to use a globalization facility.
            get { return "Cancel"; }
        }

        public static string TextOfCloseButton
        {
            //TODO: This is hard-coded just for now -- it really needs to be converted to use a globalization facility.
            get { return "Close"; }
        }

        public static string TextOfOkButton
        {
            //TODO: This is hard-coded just for now -- it really needs to be converted to use a globalization facility.
            get { return "OK"; }
        }

        public static string TextOfYesButton
        {
            //TODO: This is hard-coded just for now -- it really needs to be converted to use a globalization facility.
            get { return "Yes"; }
        }

        public static string TextOfNoButton
        {
            //TODO: This is hard-coded just for now -- it really needs to be converted to use a globalization facility.
            get { return "No"; }
        }

        public static string TextOfRetryButton
        {
            //TODO: This is hard-coded just for now -- it really needs to be converted to use a globalization facility.
            get { return "Retry"; }
        }

        public static string TextOfIgnoreButton
        {
            //TODO: This is hard-coded just for now -- it really needs to be converted to use a globalization facility.
            get { return "Ignore"; }
        }

        public static string TextOfAbortButton
        {
            //TODO: This is hard-coded just for now -- it really needs to be converted to use a globalization facility.
            get { return "Abort"; }
        }
        #endregion TextOf(the various buttons)

        #endregion unit-testing facilities

        #region ResultFrom(System.Windows.Forms.DialogResult)
#if !SILVERLIGHT
        /// <summary>
        /// Converts a System.Windows.Forms.DialogResult to our preferred MessageResult
        /// </summary>
        /// <param name="dialogResult">the System.Windows.Forms.DialogResult we want to convert</param>
        /// <returns>the coresponding MessageResult</returns>
        public static MessageResult ResultFrom(System.Windows.Forms.DialogResult dialogResult)
        {
            MessageResult r;
            switch (dialogResult)
            {
                case System.Windows.Forms.DialogResult.Abort:
                    r = MessageResult.Abort;
                    break;
                case System.Windows.Forms.DialogResult.Cancel:
                    r = MessageResult.Cancel;
                    break;
                case System.Windows.Forms.DialogResult.Ignore:
                    r = MessageResult.Ignore;
                    break;
                case System.Windows.Forms.DialogResult.No:
                    r = MessageResult.No;
                    break;
                case System.Windows.Forms.DialogResult.None:
                    // Map None to TimedOut
                    r = MessageResult.TimedOut;
                    break;
                case System.Windows.Forms.DialogResult.OK:
                    r = MessageResult.Ok;
                    break;
                case System.Windows.Forms.DialogResult.Retry:
                    r = MessageResult.Retry;
                    break;
                case System.Windows.Forms.DialogResult.Yes:
                    r = MessageResult.Yes;
                    break;
                default:
                    r = MessageResult.Close;
                    break;
            }
            return r;
        }
#endif
        #endregion

        #region ResultFrom(System.Windows.MessageBoxResult)
        /// <summary>
        /// Converts a MessageBoxResult to our preferred MessageResult
        /// </summary>
        /// <param name="messageBoxResult">the MessageBoxResult we want to convert</param>
        /// <returns>the coresponding MessageResult</returns>
        public static MessageResult ResultFrom(MessageBoxResult messageBoxResult)
        {
            MessageResult r;
            switch (messageBoxResult)
            {
                case MessageBoxResult.Yes:
                    r = MessageResult.Yes;
                    break;
                case MessageBoxResult.No:
                    r = MessageResult.No;
                    break;
                case MessageBoxResult.Cancel:
                    r = MessageResult.Cancel;
                    break;
                case MessageBoxResult.OK:
                    r = MessageResult.Ok;
                    break;
                default:
                    r = MessageResult.Close;
                    break;
            }
            return r;
        }
        #endregion

        public static void SetCustomElement(FrameworkElement element)
        {
            //cbl
            _customElement = element;
        }
        private static FrameworkElement _customElement;

        #region Dispose
        /// <summary>
        /// Release any test-specific resources and turn off test-mode.
        /// </summary>
        public void Dispose()
        {
            if (_testFacility != null)
            {
                _testFacility.Dispose();
                // By setting MessageBox's _testFacility variable to null, we tell it that it is no longer in test-mode.
                _testFacility = null;
            }
            GC.SuppressFinalize(this);
        }
        #endregion

        #region internal implementation

        #region GetWhetherButtonIsIncluded
        /// <summary>
        /// See whether the TestFacility.ButtonResultToSelect is present amongst the given MessageBoxButtons
        /// and return true if it is.
        /// </summary>
        /// <param name="buttonFlags">the buttons to check against, to see whether the one that is required is present</param>
        /// <returns>true if the ButtonResultToSelect is included within the given buttonFlags, false otherwise</returns>
        internal bool GetWhetherButtonIsIncluded(MessageBoxButtons buttonFlags, out string complaint)
        {
            complaint = String.Empty;
            bool isOkay = true;
            if (_testFacility != null)
            {
                MessageResult? desiredResult = _testFacility.ButtonResultToSelect;
                if (desiredResult.HasValue)
                {
                    switch (desiredResult)
                    {
                        case MessageResult.Abort:
                            if ((buttonFlags & MessageBoxButtons.Cancel) == 0)
                            {
                                // If testing, and the test calls for this button to be selected - complain that it won't be possible.
                                complaint = "You want the Cancel button to be selected, but that button is not being shown!";
                                isOkay = false;
                            }
                            break;
                        case MessageResult.Cancel:
                            if ((buttonFlags & MessageBoxButtons.Cancel) == 0)
                            {
                                // If testing, and the test calls for this button to be selected - complain that it won't be possible.
                                complaint = "You want the Cancel button to be selected, but that button is not being shown!";
                                isOkay = false;
                            }
                            break;
                        case MessageResult.Close:
                            if ((buttonFlags & MessageBoxButtons.Close) == 0)
                            {
                                // If testing, and the test calls for this button to be selected - complain that it won't be possible.
                                complaint = "You want the Close button to be selected, but that button is not being shown!";
                                isOkay = false;
                            }
                            break;
                        case MessageResult.Ignore:
                            if ((buttonFlags & MessageBoxButtons.Ignore) == 0)
                            {
                                // If testing, and the test calls for this button to be selected - complain that it won't be possible.
                                complaint = "You want the Ignore button to be selected, but that button is not being shown!";
                                isOkay = false;
                            }
                            break;
                        case MessageResult.No:
                            if ((buttonFlags & MessageBoxButtons.No) == 0)
                            {
                                // If testing, and the test calls for this button to be selected - complain that it won't be possible.
                                complaint = "You want the No button to be selected, but that button is not being shown!";
                                isOkay = false;
                            }
                            break;
                        case MessageResult.Ok:
                            if ((buttonFlags & MessageBoxButtons.Ok) == 0)
                            {
                                // If testing, and the test calls for this button to be selected - complain that it won't be possible.
                                complaint = "You want the Ok button to be selected, but that button is not being shown!";
                                isOkay = false;
                            }
                            break;
                        case MessageResult.Retry:
                            if ((buttonFlags & MessageBoxButtons.Retry) == 0)
                            {
                                // If testing, and the test calls for this button to be selected - complain that it won't be possible.
                                complaint = "You want the Retry button to be selected, but that button is not being shown!";
                                isOkay = false;
                            }
                            break;
                        case MessageResult.Yes:
                            if ((buttonFlags & MessageBoxButtons.Yes) == 0)
                            {
                                // If testing, and the test calls for this button to be selected - complain that it won't be possible.
                                complaint = "You want the Yes button to be selected, but that button is not being shown!";
                                isOkay = false;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            return isOkay;
        }
        #endregion GetWhetherButtonIsIncluded

        #region GetExpressionOfDismay
        /// <summary>
        /// Return a somewhat-varying English expression of dismay, drawing from stock expressions unless this has been
        /// explicitly set. This is intended specifically for heading an error-message.
        /// </summary>
        /// <returns>An English expression such as "oops" or "oh crap"</returns>
        public string GetExpressionOfDismay()
        {
            if (_defaultConfiguration != null)
            {
                if (_defaultConfiguration.DefaultCaptionForErrors != null)
                {
                    return _defaultConfiguration.DefaultCaptionForErrors;
                }
            }
            string somethingToSay = _someExpressionsOfDismay[_indexIntoExpressionsOfDismay];
            _indexIntoExpressionsOfDismay++;
            if (_indexIntoExpressionsOfDismay >= _someExpressionsOfDismay.Length)
            {
                _indexIntoExpressionsOfDismay = 0;
            }
            return somethingToSay;
        }
        #endregion

        #region GetAReasonableTitlebarPrefix
        /// <summary>
        /// Return a prefix to use in the title-bar of this message-box.
        /// </summary>
        /// <returns>A title-bar prefix that indicates the application, or else an empty-string if this is not an IDesktopApplication.</returns>
        private string GetAReasonableTitlebarPrefix()
        {
            string titlebarPrefix;
            // If this application implements IDesktopApplication, then use it's facility to get the title-bar prefix.
            var iapp = _ownerApplication as IDesktopApplication;
            if (iapp != null)
            {
                titlebarPrefix = iapp.ProductIdentificationPrefix;
            }
            else
            {
                //TODO  How to do this for SL?
#if SILVERLIGHT
                titlebarPrefix = String.Empty;
#else
                //CBL  What about the vendor-name?
 //CBL               titlebarPrefix = DesktopApplication.GetProgramName();
                titlebarPrefix = "";

                // Shorten it if it's too long..
                int n = titlebarPrefix.Length;
                const int nLimit = 64;
                if (n > nLimit)
                {
                    // Show just the last nLimit characters preceded by ".."
                    string sPartToShow = titlebarPrefix.Substring(n - nLimit - 2);
                    titlebarPrefix = sPartToShow + "..";
                }
#endif
            }
            return titlebarPrefix;
        }
        #endregion

        internal void SignalThatMessageBoxHasEnded(object sender, MessageResult result)
        {
            //TODO: May want to provide a value for error and isCancelled
            MessageBoxWindow messageBoxWindow = sender as MessageBoxWindow;
            MessageBoxConfiguration options = messageBoxWindow._options;
            options.SignalThatMessageBoxHasEnded(messageBoxWindow, result);
        }

        #region fields

        /// <summary>
        /// the MessageBoxConfiguration to use for this message-box
        /// </summary>
        private MessageBoxConfiguration _configuration;
        /// <summary>
        /// the MessageBoxDefaultConfiguration to use by default for invocations of this message-box
        /// </summary>
        private static MessageBoxDefaultConfiguration _defaultConfiguration;
        /// <summary>
        /// This is the standard prefix-text that is shown on the caption.
        /// </summary>
        private string _captionPrefix;
        /// <summary>
        /// This stores a reference to the most recent message-box window that is currently being shown.
        /// </summary>
        private MessageBoxWindow _messageBoxWindow;
        private System.Windows.Application _ownerApplication;

        private int _indexIntoExpressionsOfDismay;
        private readonly string[] _someExpressionsOfDismay = { "Oops!", "Oh man!", "Well isn't that special!", "Well, it's like this..", "Awh crap!" };

        /// <summary>
        /// This contains the set of test-options, and helper methods, that apply specifically for running automated tests.
        /// </summary>
        internal MessageBoxTestFacility _testFacility;

        #endregion fields

        #endregion internal implementation
    }
    #endregion class MessageBox
}

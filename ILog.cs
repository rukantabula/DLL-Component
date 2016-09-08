using System.Linq;
using System.Threading.Tasks;

namespace LogTest
{
    public interface ILog
    {
        /// <summary>
        /// Modifies internal paramters so the logger runs exit with flush mode
        /// </summary>
        void ExitFlushTestingMode();



        /// <summary>
        /// Modifies internal paramters so the logger runs in exit without flush mode
        /// </summary>
        void ExitNoFlushTestingMode();


        /// <summary>
        /// Release the OS lock on the file
        /// </summary>
        void ReleaseFile ();

        /// <summary>
        /// Stop the logging. If any outstadning logs theses will not be written to Log
        /// </summary>
        void StopWithoutFlush();

        /// <summary>
        /// Stop the logging. The call will not return until all all logs have been written to Log.
        /// </summary>
        void StopWithFlush();

        /// <summary>
        /// Write a message to the Log.
        /// </summary>
        /// <param name="text">The text to written to the log</param>
        void Write(string text);


    }
}

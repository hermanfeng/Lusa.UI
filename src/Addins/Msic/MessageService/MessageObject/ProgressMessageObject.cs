namespace CommonLibrary
{
    public class ProgressMessageObject:MessageObject
    {
        public ProgressMessageObject()
        {
            IsGlobalProgess = true;
            Type = MessageType.Progress;
            IsIndeterminate = false;
        }

        /// <summary>
        /// 0-100
        /// </summary>
        public int Progress { get; set; }

        public bool IsFinished
        {
            get { return this.Progress >= 100; }
        }

        /// <summary>
        /// Model diglog progress
        /// </summary>
        public bool IsGlobalProgess { get; set; }

        public bool IsIndeterminate { get; set; }
    }
}


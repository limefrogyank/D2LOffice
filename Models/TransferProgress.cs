using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;

namespace D2LOffice.Models
{
    public enum ProgressStatus
    {
        Running,
        Completed,
        Error
    }
    public class ProgressInfo
    {
        public long BytesTransferred { get; set; }
        public long? TotalBytesExpected { get; set; }
        public ProgressStatus ProgressStatus { get; set; }

        public ProgressInfo(long bytesTransferred, long? totalByt, ProgressStatus progressStatus)
        {
            BytesTransferred = bytesTransferred;
            TotalBytesExpected = totalByt;
            ProgressStatus = progressStatus;
        }
    }

    public class TransferProgress : IProgress<ProgressInfo>
    {
        public IObservable<ProgressInfo> WhenProgress => _progressSubject.AsObservable();

        Subject<ProgressInfo> _progressSubject = new Subject<ProgressInfo>();

        public void Report(ProgressInfo value)
        {
            _progressSubject.OnNext(value);

            if (value.ProgressStatus != ProgressStatus.Running)
                _progressSubject.OnCompleted();

        }
    }

    
}

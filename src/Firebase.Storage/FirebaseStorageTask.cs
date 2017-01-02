namespace Firebase.Storage
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Threading.Tasks;

    public class FirebaseStorageTask
    {
        private const int ProgressReportDelayMiliseconds = 500;

        private readonly Task uploadTask;
        private readonly Stream stream;

        public FirebaseStorageTask(FirebaseStorageOptions options, string url, Stream stream, CancellationToken cancellationToken)
        {
            this.TargetUrl = url;
            this.uploadTask = this.UploadFile(options, url, stream, cancellationToken);
            this.stream = stream;
            this.Progress = new Progress<FirebaseStorageProgress>();

            Task.Factory.StartNew(() => ReportProgressLoop());
        }

        public Progress<FirebaseStorageProgress> Progress
        {
            get;
            private set;
        }


        public string TargetUrl
        {
            get;
            private set;
        }

        public TaskAwaiter GetAwaiter()
        {
            return this.uploadTask.GetAwaiter();
        }

        private async Task UploadFile(FirebaseStorageOptions options, string url, Stream stream, CancellationToken cancellationToken)
        {
            var responseData = "N/A";

            try
            {
                using (var client = await options.CreateHttpClientAsync())
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, url)
                    {
                        Content = new StreamContent(stream)
                    };

                    var response = await client.SendAsync(request, cancellationToken).ConfigureAwait(false);
                    responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                    response.EnsureSuccessStatusCode();
                }
            }
            catch (TaskCanceledException)
            {
                if (options.ThrowOnCancel)
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                throw new FirebaseStorageException(url, responseData, ex);
            }
        }

        private async void ReportProgressLoop()
        {
            while (!this.uploadTask.IsCompleted)
            {
                await Task.Delay(ProgressReportDelayMiliseconds);
                
                this.OnReportProgress(new FirebaseStorageProgress(this.stream.Position, this.stream.Length));
            }
        }

        private void OnReportProgress(FirebaseStorageProgress progress)
        {
            (this.Progress as IProgress<FirebaseStorageProgress>).Report(progress);
        }
    }
}

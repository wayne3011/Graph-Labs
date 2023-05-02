namespace GraphLab
{
    class MultiWriter
    {
        StreamWriter[] streams;
        public MultiWriter(IEnumerable<Stream> streams)
        {
            this.streams = new StreamWriter[streams.Count()];
            int i = 0;
            
            foreach (Stream stream in streams)
            {
                this.streams[i] = new StreamWriter(stream);
                //this.streams[i].Flush();
                this.streams[i].AutoFlush = true;
                i++;
            }
        }

        public void WriteLine(string value)
        {
            foreach(StreamWriter stream in streams)
            {
                stream.WriteLine(value);
            }
        }
        public void CloseAll()
        {
            for (int i = 0; i < streams.Length; i++)
            {
                streams[i].Dispose();
            }
            streams = new StreamWriter[0];
        }
    }
}

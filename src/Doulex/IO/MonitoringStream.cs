using Doulex.Performance;

namespace Doulex.IO;

/// <summary>
/// A monitoring stream or called pass through stream, it used to intercept the data and calculate the data rate.
/// </summary>
public class MonitoringStream : Stream
{
    private readonly Stream _baseStream;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="baseStream"></param>
    public MonitoringStream(Stream baseStream)
    {
        _baseStream = baseStream;
    }

    /// <summary>
    /// The stats of the stream
    /// </summary>
    public MonitoringStreamCounter Counter { get; } = new();

    /// <inheritdoc />
    public override long Seek(long offset, SeekOrigin origin)
    {
        return _baseStream.Seek(offset, origin);
    }

    /// <inheritdoc />
    public override void SetLength(long value)
    {
        _baseStream.SetLength(value);
    }

    /// <summary>When overridden in a derived class, writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.</summary>
    /// <param name="buffer">An array of bytes. This method copies count bytes from buffer to the current stream.</param>
    /// <param name="offset">The zero-based byte offset in buffer at which to begin copying bytes to the current stream.</param>
    /// <param name="count">The number of bytes to be written to the current stream.</param>
    /// <exception cref="T:System.ArgumentException">The sum of <paramref name="offset">offset</paramref> and <paramref name="count">count</paramref> is greater than the buffer length.</exception>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="buffer">buffer</paramref> is null.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="offset">offset</paramref> or <paramref name="count">count</paramref> is negative.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occured, such as the specified file cannot be found.</exception>
    /// <exception cref="T:System.NotSupportedException">The stream does not support writing.</exception>
    /// <exception cref="T:System.ObjectDisposedException"><see cref="M:System.IO.Stream.Write(System.Byte[],System.Int32,System.Int32)"></see> was called after the stream was closed.</exception>
    public override void Write(byte[] buffer, int offset, int count)
    {
        _baseStream.Write(buffer, offset, count);

        if (count > 0)
        {
            Counter.Sent.AddSample(count);
        }
    }

    /// <summary>When overridden in a derived class, reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.</summary>
    /// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array with the values between offset and (offset + count - 1) replaced by the bytes read from the current source.</param>
    /// <param name="offset">The zero-based byte offset in buffer at which to begin storing the data read from the current stream.</param>
    /// <param name="count">The maximum number of bytes to be read from the current stream.</param>
    /// <returns>The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.</returns>
    /// <exception cref="T:System.ArgumentException">The sum of <paramref name="offset">offset</paramref> and <paramref name="count">count</paramref> is larger than the buffer length.</exception>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="buffer">buffer</paramref> is null.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="offset">offset</paramref> or <paramref name="count">count</paramref> is negative.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
    /// <exception cref="T:System.NotSupportedException">The stream does not support reading.</exception>
    /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
    public override int Read(byte[] buffer, int offset, int count)
    {
        int bytesRead = _baseStream.Read(buffer, offset, count);

        if (bytesRead > 0)
        {
            Counter.Received.AddSample(bytesRead);
        }

        return bytesRead;
    }

    /// <inheritdoc />
    public override bool CanRead => _baseStream.CanRead;

    /// <inheritdoc />
    public override bool CanSeek => _baseStream.CanSeek;

    /// <inheritdoc />
    public override bool CanWrite => _baseStream.CanWrite;

    /// <inheritdoc />
    public override long Length => _baseStream.Length;

    /// <inheritdoc />
    public override long Position
    {
        get => _baseStream.Position;
        set => _baseStream.Position = value;
    }

    /// <inheritdoc />
    public override void Flush()
    {
        _baseStream.Flush();
    }
}

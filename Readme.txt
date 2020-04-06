In this demo there is a server that takes note of addresses sent by the client,
using it to call that specific client through a remote object hosted in the client.
The server exposes a singleton remote object where the client address is first
communicated. A second method in this object calls back the corresponding client,
and only it.
The client has also a remote object callable from the server, and modifying the
interface (through Control.BeginInvoke(...)). In order to be able to have several
clients in the same machine, the ports of this remote object should be different
and allocated dynamically.

Try to run a single server and several clients. For each client register first its
address a single time, and then call the server (any number of times and interleaved
with other clients).

The remote object exposed by the client is constructed by the client, with a reference
of its window passed to it. In this way remote calls made by the server can modify
the GUI. Take care when doing this, as the remote calls are done in different threads.
 
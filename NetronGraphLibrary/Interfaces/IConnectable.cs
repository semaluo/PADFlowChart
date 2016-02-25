namespace Netron.GraphLib.Interfaces
{
    /// <summary>
    /// This interface used to create customed Connection
    /// </summary>
    public interface IConnectable
    {
        /// <summary>
        /// Create connection according to the connector by shape
        /// GraphicControl.OnMouseDown will check if a shape realizes this interface,
        /// if yes, create the connection according to which connector is connected. 
        /// </summary>
        /// <param name="connector"></param>
        /// <returns></returns>
        Connection CreateConnection(Connector connector);
    }
}
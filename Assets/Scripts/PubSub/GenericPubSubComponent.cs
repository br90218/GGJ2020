namespace PubSubCommunication
{
    using UnityEngine;

    /// <summary>
    /// Defines GenericPubSubCompnent that all other PubSubComponents will inherit from.
    /// This is the basis that all this components will need.
    /// </summary>
    public class GenericPubSubComponent : MonoBehaviour
    {
        public static PubSubServer PubSubServerInstance
        {
            get
            {
                return PubSubServer.Instance;
            }
        }
    }
}

namespace Sandbox.Core {
    public class ModuleCommunicationData {

        #region Properties
        
        public object Data { get; set; }
        public ModuleCommunicationType CommunicationType { get; set; }

        #endregion

        #region Constructors

        public ModuleCommunicationData(ModuleCommunicationType communicationType, object data) {
            CommunicationType = communicationType;
            Data = data;
        }

        #endregion

    }
}
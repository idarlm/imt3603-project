using Unity.VisualScripting;
using System.Linq;

namespace EventSystem
{
    /// <summary>
    /// TriggerCollection keeps track of the status of each provided ID.
    /// </summary>
    public class TriggerCollection
    {
        private (int, bool)[] _triggerStatus;
        private int _activeCount;

        public TriggerCollection(int[] ids)
        {
            _triggerStatus = new (int, bool)[ids.Length];
            for (var i = 0; i < ids.Length; i++)
            {
                _triggerStatus[i] = (ids[i], false);
            }
        }


        /// <summary>
        /// AllActive returns true if all triggers are active
        /// </summary>
        /// <returns></returns>
        public bool AllActive()
        {
            return _triggerStatus.All(trigger => trigger.Item2);
        }

        /// <summary>
        /// AnyActive returns true if any trigger is active.
        /// </summary>
        /// <returns></returns>
        public bool AnyActive()
        {
            return _triggerStatus.Any(trigger => trigger.Item2);
        }

        
        /// <summary>
        /// GetActiveCount returns how many triggers are currently active
        /// </summary>
        /// <returns>Currently active triggers</returns>
        public int GetActiveCount()
        {
            return _activeCount;
        }

        /// <summary>
        /// GetInactiveCount returns how many triggers are currently not triggered
        /// </summary>
        /// <returns>Currently inactive triggers</returns>
        public int GetInactiveCount()
        {
            return _triggerStatus.Length - _activeCount;
        }

        
        /// <summary>
        /// SetStatus sets the status of a trigger.
        /// </summary>
        /// <param name="id">id of trigger</param>
        /// <param name="active">true for an active trigger</param>
        /// <returns>true if a status was changed</returns>
        public bool SetStatus(int id, bool active)
        {
            for (var i = 0; i < _triggerStatus.Length; i++)
            {
                if (_triggerStatus[i].Item1 == id)
                {
                    var activationChange = _triggerStatus[i].Item2 != active;
                    _triggerStatus[i].Item2 = active;
                    if (activationChange)
                    {
                        _activeCount += active? -1 : 1;
                    }
                    return activationChange;
                }
            }
            return false;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Events
{
    public class EventHandler : MonoBehaviour
    {
        public virtual void handleLookAtMe(Vector3 lookPosition) { throw new NotImplementedException();  }
    }
}

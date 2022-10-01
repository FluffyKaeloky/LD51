using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class FartShockwaveReceiver : MonoBehaviour
{
    public UnityEvent onReceivedShockwave = new UnityEvent();

    public void ReceiveShockwave()
    {
        onReceivedShockwave?.Invoke();
    }
}

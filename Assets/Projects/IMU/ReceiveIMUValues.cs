using UnityEngine;
using System.Collections;
using  System.IO.Ports;
using System.Text;

public class ReceiveIMUValues : MonoBehaviour {

    Vector3 position;
    Vector3 rotation;
    public Vector3 rotationOffset ;
    public float speedFactor = 5.0f;

    string serial_string = "";

    SerialPort sp = new SerialPort("/dev/cu.usbserial-14620", 40000); 

    void Start () {
        sp.Open();
       //sp.WriteTimeout = 100;
        sp.ReadTimeout = 300; 
        
       
    }

    void Update()
    {
        //Debug.Log("The rotation is : " + transform.Find("IMU_Object").eulerAngles);

        if(!sp.IsOpen){
            sp.Open();
        }else{
            sp.Write("q");
            serial_string = sp.ReadLine(); 
            Debug.Log("QUATERNION String:" +serial_string); 
            ReadQuaternion(serial_string);

            this.transform.Translate(0,0,Time.deltaTime * 0.01f);

            //  sp.Write("y");
            //  serial_string = sp.ReadLine(); 
            //  Debug.Log("YAWPITCHROLL String:" +serial_string); 
            //  ReadyawPitchRoll(serial_string);
         
            
        }
    }

    void ReadQuaternion (string data) {
        string[] values = data.Split('/');
        Debug.Log("string.length:"+ values.Length);
        if (values.Length == 4) // Rotation
        {
            float w = float.Parse(values[0]);
            float x = float.Parse(values[1]);
            float y = float.Parse(values[2]);
            float z = float.Parse(values[3]);
            this.transform.localRotation 
            = Quaternion.Lerp(this.transform.localRotation,  new Quaternion(w, y, x, z), Time.deltaTime * speedFactor);
        }

        this.transform.parent.transform.eulerAngles = rotationOffset;
    }


        void ReadyawPitchRoll (string data) {
        string[] values = data.Split('/');
        Debug.Log("string.length:"+ values.Length);
        if (values.Length == 3) // Euler
        {
            float yaw = float.Parse(values[0]);
            float pitch = float.Parse(values[1]);
            float roll  = float.Parse(values[2]);
   
            this.transform.position = new Vector3(pitch,yaw,roll); 
        }

    }
  } 


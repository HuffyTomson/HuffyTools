#region INPUTW
////////////////////////////////////////////////////////////////////////////////
// InputW
// Wrapper class for UnityEngine.Input. Allows for touch and mouse input using
// the singleton pattern. The instance looks for what platform you are running 
// and sets delegate functions to use the right input type. 
////////////////////////////////////////////////////////////////////////////////
#endregion
#region USING
using UnityEngine;
using System.Collections;
#endregion
#region INPUTTYPE
public enum InputType
{
   mobile,
   pc
};
#endregion
#region PUBLIC GLOBAL MENMERS
// input delegates //
public delegate Vector2 inputPosDelegate(int num);
public delegate bool inputDownDelegate(int num);
public delegate bool inputUpDelegate(int num);
public delegate Vector2 inputDeltaDelegate(int num);
public delegate Vector2 inputAxisDeligate();
public delegate float inputScalingDeligate();
#endregion
public class InputW
{
    #region MEMBERS

    static Vector2 lastPosition = Vector2.zero;

    public inputPosDelegate GetInputPos;
    public inputDownDelegate GetInputDown;
    public inputUpDelegate GetInputUp;
    public inputDeltaDelegate GetInputDelta;
    public inputAxisDeligate GetAxis;
    public inputScalingDeligate GetScaling;

    // hold what type of input you are using 
    private InputType inputType;
    public InputType SetInputType
    {
       get { return inputType; }

       set
       {
          inputType = value;

          switch (inputType)
          {
             case InputType.mobile:
                GetInputPos = InputW.TouchPos;
                GetInputDown = InputW.TouchDown;
                GetInputUp = InputW.TouchUp;
                GetInputDelta = InputW.TouchDelta;
                GetAxis = InputW.TouchPan;
                GetScaling = InputW.TouchPinch;
                break;
             case InputType.pc:
                GetInputPos = InputW.MousePos;
                GetInputDown = InputW.MouseDown;
                GetInputUp = InputW.MouseUp;
                GetInputDelta = InputW.MouseDelta;
                GetAxis = InputW.KeyboardAxis;
                GetScaling = InputW.ScrollWheel;
                break;
          }
       }
    }

    // singleton //
    private static InputW instance;
    public static InputW Instance 
    {
        get 
        {
            if (instance == null)
            {
                instance = new InputW();

                // test for what platform you are on
                if (Application.platform == RuntimePlatform.Android ||
                    Application.platform == RuntimePlatform.IPhonePlayer)
                {
                   instance.SetInputType = InputType.mobile;
                }
                else
                {
                   instance.SetInputType = InputType.pc;
                }
            }

            return instance; 
        } 
    }

    #endregion
    ////////////////////////////////////////////////////////////////////////////////
    // Accelerometer //
    ////////////////////////////////////////////////////////////////////////////////
    #region ACCLEROMETOR
    // holds default accelerometer orientation
    private static Matrix4x4 calibrationMatrix = Matrix4x4.identity;
    public static void CalibrateAccelerometer()
    {
        // rotation of 
        Quaternion rotateQuaternion = Quaternion.FromToRotation(new Vector3(0.0f, -1.0f, 0.0f), Input.acceleration);
        //create identity matrix ... rotate our matrix to match up with down vec
        Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, rotateQuaternion, new Vector3(1.0f, 1.0f, 1.0f));
        //get the inverse of the matrix
        calibrationMatrix = matrix.inverse;
    }
    private static Vector3 FixAcceleration(Vector3 accelerator)
    {
        return calibrationMatrix.MultiplyVector(accelerator);
    }
    public Vector3 GetAcceleration()
    {
        return FixAcceleration(Input.acceleration);
    }
    #endregion
    ////////////////////////////////////////////////////////////////////////////////
    // touch & mouse //
    ////////////////////////////////////////////////////////////////////////////////
    #region TOUCH MOUSE
    // 
    static Vector2 lastMousePosition = Vector2.zero;

    // position //
    private static Vector2 MousePos(int num)
    {
        return Input.mousePosition;
    }
    private static Vector2 TouchPos(int num)
    {
        if (Input.touchCount > num)
            lastPosition = Input.GetTouch(num).position;

        return lastPosition;

    }
    // down //
    private static bool MouseDown(int num)
    {
        return Input.GetMouseButtonDown(num);
    }
    private static bool TouchDown(int num)
    {
        if (Input.touchCount > num && Input.GetTouch(num).phase == TouchPhase.Began)
            return true;
        else
            return false;
    }
    // up //
    private static bool MouseUp(int num)
    {
        return Input.GetMouseButtonUp(num);
    }
    private static bool TouchUp(int num)
    {
        if (Input.touchCount > num && Input.GetTouch(num).phase == TouchPhase.Ended)
            return true;
        else
            return false;
    }

    // move delta //
    private static Vector2 MouseDelta(int num)
    {
        if (lastMousePosition == Vector2.zero)
        {
            lastMousePosition = Input.mousePosition;
            return Vector2.zero;
        }
        if (Input.GetMouseButton(num))
        {
            Vector2 mouseDelta = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - lastMousePosition;
            lastMousePosition = Input.mousePosition;

            if (mouseDelta.magnitude < 100)
                return mouseDelta;
            else
                return Vector2.zero;
        }
        // else
        return Vector2.zero;
    }
    private static Vector2 TouchDelta(int num)
    {
        if (Input.touchCount > num)
        {
            return Input.GetTouch(num).deltaPosition;
        }
        // else
        return Vector2.zero;
    }


#endregion
    ////////////////////////////////////////////////////////////////////////////////
    // pan //
    ////////////////////////////////////////////////////////////////////////////////
#region PAN
    private static Vector2 TouchPan()
    {
        if (Input.touchCount > 1 && Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved)
        {
            // Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            Vector2 startPosition = (touchZeroPrevPos + touchOnePrevPos) / 2;
            Vector2 endPosition = (touchZero.position + touchOne.position) / 2;
            
            return (startPosition - endPosition) * 0.1f;
        }

        return Vector2.zero;
    }

    private static Vector2 KeyboardAxis()
    {
        return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }
#endregion
    ////////////////////////////////////////////////////////////////////////////////
    // scaling input //
    ////////////////////////////////////////////////////////////////////////////////
#region SCALING
    private static float ScrollWheel()
    {
        // multiplied the axis to better match the return from TouchPinch() 
        return -Input.GetAxis("Mouse ScrollWheel") * 200;
    }

    private static float TouchPinch()
    {
        if (Input.touchCount > 1 && Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved)
        {
            // Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            return (prevTouchDeltaMag - touchDeltaMag);
        }
        else
        {
            return 0;
        }
    }
    #endregion

}

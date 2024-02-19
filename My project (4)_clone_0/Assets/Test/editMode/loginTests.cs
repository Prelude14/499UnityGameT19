using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class loginTests
{
    private readInput loginScript; //going to test the readInput script output using different usernames and passwords
    private InputField usernameInputField;
    private InputField passwordInputField;
    private bool loginCompleted; //needed to simulate proper time for the login co routine to start

    //========================SETUP TEST USERNAMES AND PASSWORDS=======================

    //supply short strings for the username and password, which are ALSO INVALID
    string shortUsername = "john@d";
    string shortPassword = "johnd";
    //supply long enough string for the username (>=10 characters), but use short pass still --NEED VALID USERNAME in order to produce proper error code.
    string longUsername = "john@doe.com";
    //supply short username WITH VALID pass now (>=10 characters)
    string longPass = "johndoe123"; //--NEED VALID PASSWORD in order to produce proper error code.
    //supply TOO LONG string for the username (>30 characters),
    string toolongU = "john@doe.comisnotlongenoughsoimadeitlonger";
    //supply TOO LONG pass now (>30 characters), which are ALSO INVALID
    string toolongP = "johndoe123isnotlongenoughsoimadeitlonger";

    [SetUp]
    public void SetUp()
    {
        //Need to set up a new gameobject that will run our tests on the readInput.cs script's Login IEnumerator function
        GameObject gameObject = new GameObject();
        loginScript = gameObject.AddComponent<readInput>();

        // Create InputFields for the custom test username and password values that are going to be tested
        GameObject usernameGO = new GameObject("UsernameInput");
        usernameInputField = usernameGO.AddComponent<InputField>();
        loginScript.username_email = usernameInputField;

        GameObject passwordGO = new GameObject("PasswordInput");
        passwordInputField = passwordGO.AddComponent<InputField>();
        loginScript.user_pass = passwordInputField;

        // Reset any initial text in the input fields
        usernameInputField.text = "";
        passwordInputField.text = "";


    }

    // Helper method to wait for the coroutine to complete
    IEnumerator WaitForCoroutineCompletion(readInput script)
    {
        while (!script.IsLoginCoroutineCompleted())
        {
            yield return null;
        }
    }

    // Test if the username is long enough, is a valid email, as well as check the password's length (not worried about specific password features yet)
    //==================================================================================== SHORT string tests ===========================================================================================
    [UnityTest]
    public IEnumerator ValidateLogin_inputShortB()
    {
        // Arrange: Set up test data with INVLAID INPUTS
        loginScript.username_email.text = shortUsername;
        loginScript.user_pass.text = shortPassword;

        // Act: Call the login function using the SHORT STRINGS
        loginScript.CallLogin();

        // Simulate the www response - assuming the SERVER WILL RETURN AN ERROR CODE (login.php 's echo message)
        string wwwResponse = "3: Invalid email format, the inputted username is not a valid email address."; // Expected error code since the username is not a valid email, and that error should trigger first

        yield return null; // wait for ONE frame to simulate the time for the login coroutine to start

        // Assert: Check if the correct failure message was logged
        LogAssert.Expect(LogType.Log, "User logged FAILED. Error Code: " + wwwResponse); //CHECK IF FAILED in general, AND exactly why it failed
        // Add more assertions as needed

        // Wait for the coroutine to complete
        yield return WaitForCoroutineCompletion(loginScript);

        // Additional assertions related to UI changes or behaviors after successful login can be added here
    }

    [UnityTest]
    public IEnumerator ValidateLogin_inputShortPassOnly()
    {
        loginScript.username_email.text = longUsername; //VALID USER
        loginScript.user_pass.text = shortPassword; //SHORT PASS

        // Act: Call the login function using the SHORT STRINGS
        loginScript.CallLogin();

        // Simulate the www response - assuming the SERVER WILL RETURN AN ERROR CODE (login.php 's echo message)
        string wwwResponse = "6: Incorrect password"; // Expected error code since the username IS VALID, but PASS ISN'T

        yield return null; // wait for ONE frame to simulate the time for the login coroutine to start

        // Assert: Check if the correct failure message was logged
        LogAssert.Expect(LogType.Log, "User logged FAILED. Error Code: " + wwwResponse); //CHECK IF FAILED in general, AND EXACTLY WHY
        // Add more assertions as needed

        // Wait for the coroutine to complete
        yield return WaitForCoroutineCompletion(loginScript);

        // Additional assertions related to UI changes or behaviors after successful login can be added here
    }
    
    [UnityTest]
    public IEnumerator ValidateLogin_inputShortUserOnly()
    {
        loginScript.username_email.text = shortUsername; //SHORT USER
        loginScript.user_pass.text = longPass; //VALID PASSWORD

        // Act: Call the login function using the SHORT STRINGS
        loginScript.CallLogin();

        // Simulate the www response - assuming the SERVER WILL RETURN AN ERROR CODE (login.php 's echo message)
        string wwwResponse = "3: Invalid email format, the inputted username is not a valid email address."; // Expected error code since the username is not a valid email

        yield return null; // wait for ONE frame to simulate the time for the login coroutine to start

        // Assert: Check if the correct failure message was logged
        LogAssert.Expect(LogType.Log, "User logged FAILED. Error Code: " + wwwResponse); //CHECK IF FAILED in general, AND EXACTLY WHY
        // Add more assertions as needed

        // Wait for the coroutine to complete
        yield return WaitForCoroutineCompletion(loginScript);

        // Additional assertions related to UI changes or behaviors after successful login can be added here

    }
  
    //==================================================================================== LONG string tests ===========================================================================================
    [UnityTest]
    public IEnumerator ValidateLogin_inputTooLongB()
    {
        //CHECK using INVALID BOTH (TOO LONG)
        loginScript.username_email.text = toolongU; //too long USER
        loginScript.user_pass.text = toolongP; //too long PASSWORD

        // Act: Call the login function using the SHORT STRINGS
        loginScript.CallLogin();

        // Simulate the www response - assuming the SERVER WILL RETURN AN ERROR CODE (login.php 's echo message)
        string wwwResponse = "5: 0 or more than 1 user was found with that username."; // Expected error code since the username technically contains the valid email, but its not a perfect match, and that error should trigger first

        yield return null; // wait for ONE frame to simulate the time for the login coroutine to start

        // Assert: Check if the correct failure message was logged
        LogAssert.Expect(LogType.Log, "User logged FAILED. Error Code: " + wwwResponse); //CHECK IF FAILED in general, AND EXACTLY WHY
        // Add more assertions as needed

        // Wait for the coroutine to complete
        yield return WaitForCoroutineCompletion(loginScript);

        // Additional assertions related to UI changes or behaviors after successful login can be added here
    }
    [UnityTest]
    public IEnumerator ValidateLogin_inputTooLongPassOnly()
    {
        //CHECK using INVALID BOTH (TOO LONG)
        loginScript.username_email.text = longUsername; //VALID USER
        loginScript.user_pass.text = toolongP; //too long PASSWORD

        // Act: Call the login function using the SHORT STRINGS
        loginScript.CallLogin();

        // Simulate the www response - assuming the SERVER WILL RETURN AN ERROR CODE (login.php 's echo message)
        string wwwResponse = "6: Incorrect password"; // Expected error code since the username valid but the pass is incorrect

        yield return null; // wait for ONE frame to simulate the time for the login coroutine to start

        // Assert: Check if the correct failure message was logged
        LogAssert.Expect(LogType.Log, "User logged FAILED. Error Code: " + wwwResponse); //CHECK IF FAILED in general, AND EXACTLY WHY
        // Add more assertions as needed

        // Wait for the coroutine to complete
        yield return WaitForCoroutineCompletion(loginScript);

        // Additional assertions related to UI changes or behaviors after successful login can be added here
    }
    [UnityTest]
    public IEnumerator ValidateLogin_inputTooLongUserOnly()
    {
        //CHECK using INVALID USER only (TOO LONG)
        loginScript.username_email.text = toolongU; //too long USER
        loginScript.user_pass.text = longPass; //VALID PASSWORD

        // Act: Call the login function using the SHORT STRINGS
        loginScript.CallLogin();

        // Simulate the www response - assuming the SERVER WILL RETURN AN ERROR CODE (login.php 's echo message)
        string wwwResponse = "5: 0 or more than 1 user was found with that username."; // Expected error code since the username technically contains the valid email, but its not a perfect match, and that error should trigger first

        yield return null; // wait for ONE frame to simulate the time for the login coroutine to start

        // Assert: Check if the correct failure message was logged
        LogAssert.Expect(LogType.Log, "User logged FAILED. Error Code: " + wwwResponse); //CHECK IF FAILED in general, AND EXACTLY WHY
        // Add more assertions as needed

        // Wait for the coroutine to complete
        yield return WaitForCoroutineCompletion(loginScript);

        // Additional assertions related to UI changes or behaviors after successful login can be added here
    }

    //==================================================================================== VALID ACCOUNT tests ===========================================================================================
    // Test the submit button with an EXISTING account's information (one we know is in the database)
    [UnityTest]
    public IEnumerator ValidateLogin_inputValidUser()
    {
        //CHECK using VALID BOTH (Existing Account)
        loginScript.username_email.text = longUsername; //VALID USER -- "john@doe.com"
        loginScript.user_pass.text = longPass; //VALID PASSWORD -- "johndoe123"

        // Act: Call the login function using the SHORT STRINGS
        loginScript.CallLogin();

        // Simulate the www response - assuming the SERVER WILL RETURN AN ERROR CODE (login.php 's echo message)
        string wwwResponse = "0\t2023-11-23\t0\t0\t0"; // Expected error code since the username is not a valid email, and that error should trigger first

        yield return null; // wait for ONE frame to simulate the time for the login coroutine to start

        // Assert: Check if the correct failure message was logged
        LogAssert.Expect(LogType.Log, "User logged in successfully. Account Values: " + wwwResponse); //CHECK IF FAILED in general, AND EXACTLY WHY
        // Add more assertions as needed

        // Wait for the coroutine to complete
        yield return WaitForCoroutineCompletion(loginScript);

        // Additional assertions related to UI changes or behaviors after successful login can be added here
        //User logged in successfully. Account Values: 0	2023-11-23	0	0	0
        //User logged in successfully. Account Values: 0	2023-11-23	0	0	0
    }


    [TearDown]
    public void TearDown()
    {
        // Clean up all the created objects once the tests are done
        Object.DestroyImmediate(loginScript.gameObject);
        Object.DestroyImmediate(usernameInputField.gameObject);
        Object.DestroyImmediate(passwordInputField.gameObject);
    }

}

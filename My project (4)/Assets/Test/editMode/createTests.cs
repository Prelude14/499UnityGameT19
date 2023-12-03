using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class createTests
{
    private readInput createScript; //going to test the readInput script output using different usernames and passwords
    private InputField usernameInputField;
    private InputField passwordInputField;
    private InputField passwordInputField2;//need second password field
    private bool createCompleted; //needed to simulate proper time for the login co routine to start

    //========================SETUP TEST USERNAMES AND PASSWORDS=======================

    //supply short strings for the username and password,
    string shortUsername = "jon@d.com"; //valid email, but too short
    string shortUsernameInvalid = "john@d";//invalid while also being too short

    string shortPassword = "johnd";
    string secondPasswordInvalid = "johndoe"; //close to correct but not complete still, while also being different from any other versions of the password

    //supply VALID USERNAME in order to produce proper error code.
    string existingUsername = "john@doe.com";
    //supply VALID USERNAME in order to produce proper error code.
    string validPassword = "johndoe123"; //--NEED VALID PASSWORD in order to produce proper error code.

    //supply TOO LONG string for the username (>30 characters),
    string toolongU = "john@doe.comisnotlongenoughsoimadeitlonger";
    //supply TOO LONG pass now (>30 characters), which are ALSO INVALID
    string toolongP = "johndoe123isnotlongenoughsoimadeitlonger";

    //supply valid NEW username for creating new user
    string newUser = "spiderman@spider.com";
    

    [SetUp]
    public void SetUp()
    {
        //Need to set up a new gameobject that will run our tests on the readInput.cs script's Login IEnumerator function
        GameObject gameObject = new GameObject();
        createScript = gameObject.AddComponent<readInput>();

        // Create InputFields for the custom test username and password values that are going to be tested
        GameObject usernameGO = new GameObject("UsernameInput");
        usernameInputField = usernameGO.AddComponent<InputField>();
        createScript.c_username_email = usernameInputField;

        GameObject passwordGO = new GameObject("PasswordInput");
        passwordInputField = passwordGO.AddComponent<InputField>();
        createScript.c_user_pass = passwordInputField;

        GameObject password2GO = new GameObject("PasswordInput2"); //SECOND PASSWORD ENTRY
        passwordInputField2 = password2GO.AddComponent<InputField>();
        createScript.c_user_pass2 = passwordInputField2;

        // Reset any initial text in the input fields
        usernameInputField.text = "";
        passwordInputField.text = "";
        passwordInputField2.text = "";


    }

    // Helper method to wait for the coroutine to complete
    IEnumerator WaitForCoroutineCompletion(readInput script)
    {
        while (!script.IsCreateCoroutineCompleted()) //call readInput.IsCreateCoroutineCompleted() method
        {
            yield return null;
        }
    }

    // Test if the username is long enough, is a valid email, as well as check the password's length (not worried about specific password features yet)

    //================================================================================= SHORT INVALID USERNAME tests ===========================================================================================
    //6 different versions of the input to check, SHOULD ALL OUTPUT error code 3, since its the earliest error code that will be produced --the email isn't valid in all 6 tests
    [UnityTest]
    public IEnumerator ValidateCreate_inputShortInvalidU_withDiffPasses_0ValidP()  //With all 3 three fields being invalid, and the second password NOT matching the first                      **** SIU T1
    {
        // Set up test data with INVALID INPUTS, AND MISMATCHED PASSWORDS
        createScript.c_username_email.text = shortUsernameInvalid;
        createScript.c_user_pass.text = shortPassword;
        createScript.c_user_pass2.text = secondPasswordInvalid;

        // Call the create function using the SHORT STRINGS
        createScript.CallCreate();

        // Simulate the www response - assuming the SERVER WILL RETURN AN ERROR CODE (create.php 's echo message)
        string wwwResponse = "3: Invalid email format, the inputted username is not a valid email address."; // Expected error code since the username is not a valid email, and that error should trigger first

        yield return null; // wait for ONE frame to simulate the time for the create coroutine to start

        // Assert: Check if the correct failure message was logged
        LogAssert.Expect(LogType.Log, "User create FAILED. Error Code: " + wwwResponse); //CHECK IF FAILED in general, AND exactly why it failed (Username invalid error code SHOULD happen first)
        // Add more assertions as needed

        // Wait for the coroutine to complete
        yield return WaitForCoroutineCompletion(createScript);

        // Additional assertions related to UI changes or behaviors after successful create and login can be added here
    }
    [UnityTest]
    public IEnumerator ValidateCreate_inputShortInvalidU_withSamePasses_0ValidP()  //With all 3 three fields being invalid, but MATCHING PASSES                                                  **** SIU T2
    {
        // Set up test data with INVALID INPUTS, BUT MATCHED PASSWORDS
        createScript.c_username_email.text = shortUsernameInvalid; //invalid and short email
        createScript.c_user_pass.text = shortPassword; //matching short passes
        createScript.c_user_pass2.text = shortPassword;

        // Call the create function using the SHORT STRINGS
        createScript.CallCreate();

        // Simulate the www response - assuming the SERVER WILL RETURN AN ERROR CODE (create.php 's echo message)
        string wwwResponse = "3: Invalid email format, the inputted username is not a valid email address."; // Expected error code since the username is not a valid email, and that error should STILL trigger first

        yield return null; // wait for ONE frame to simulate the time for the create coroutine to start

        // Assert: Check if the correct failure message was logged
        LogAssert.Expect(LogType.Log, "User create FAILED. Error Code: " + wwwResponse); //CHECK IF FAILED in general, AND exactly why it failed (Username invalid error code SHOULD happen first)
        // Add more assertions as needed

        // Wait for the coroutine to complete
        yield return WaitForCoroutineCompletion(createScript);

        // Additional assertions related to UI changes or behaviors after successful create and login can be added here
    }
    [UnityTest]
    public IEnumerator ValidateCreate_inputShortInvalidU_withDiffPasses_1ValidP()  //With 2 fields being invalid (user and 2nd pass), but DIFFERENT PASSES                                      **** SIU T3
    {
        // Set up test data with INVALID INPUTS, BUT MISMATCHED PASSWORDS (and one is valid
        createScript.c_username_email.text = shortUsernameInvalid; //invalid and short email
        createScript.c_user_pass.text = validPassword; //valid pass
        createScript.c_user_pass2.text = shortPassword; //mismatched 2nd pass that is also invalid

        // Call the create function using the SHORT STRINGS
        createScript.CallCreate();

        // Simulate the www response - assuming the SERVER WILL RETURN AN ERROR CODE (create.php 's echo message)
        string wwwResponse = "3: Invalid email format, the inputted username is not a valid email address."; // Expected error code since the username is not a valid email, and that error should STILL trigger first

        yield return null; // wait for ONE frame to simulate the time for the create coroutine to start

        // Assert: Check if the correct failure message was logged
        LogAssert.Expect(LogType.Log, "User create FAILED. Error Code: " + wwwResponse); //CHECK IF FAILED in general, AND exactly why it failed (Username invalid error code SHOULD happen first)
        // Add more assertions as needed

        // Wait for the coroutine to complete
        yield return WaitForCoroutineCompletion(createScript);

        // Additional assertions related to UI changes or behaviors after successful create and login can be added here
    }
    [UnityTest]
    public IEnumerator ValidateCreate_inputShortInvalidU_withSamePasses_2ValidP()  //With 1 field being invalid (user), but MATCHING VALID PASSES                                               **** SIU T4
    {
        // Set up test data with INVALID INPUTS, BUT MISMATCHED PASSWORDS (and one is valid
        createScript.c_username_email.text = shortUsernameInvalid; //invalid and short email
        createScript.c_user_pass.text = validPassword; //valid pass
        createScript.c_user_pass2.text = validPassword; //mismatched 2nd pass that is also invalid

        // Call the create function using the SHORT STRINGS
        createScript.CallCreate();

        // Simulate the www response - assuming the SERVER WILL RETURN AN ERROR CODE (create.php 's echo message)
        string wwwResponse = "3: Invalid email format, the inputted username is not a valid email address."; // Expected error code since the username is not a valid email, and that error should STILL trigger first

        yield return null; // wait for ONE frame to simulate the time for the create coroutine to start

        // Assert: Check if the correct failure message was logged
        LogAssert.Expect(LogType.Log, "User create FAILED. Error Code: " + wwwResponse); //CHECK IF FAILED in general, AND exactly why it failed (Username invalid error code SHOULD happen first)
        // Add more assertions as needed

        // Wait for the coroutine to complete
        yield return WaitForCoroutineCompletion(createScript);

        // Additional assertions related to UI changes or behaviors after successful create and login can be added here
    }
    [UnityTest]
    public IEnumerator ValidateCreate_inputShortInvalidU_withDiffPasses_toolongP()  //With all 3 three fields being invalid, and the second password NOT matching the first                      **** SIU T5
    {
        // Set up test data with INVALID INPUTS, AND MISMATCHED PASSWORDS that are too long
        createScript.c_username_email.text = shortUsernameInvalid;
        createScript.c_user_pass.text = toolongP;
        createScript.c_user_pass2.text = secondPasswordInvalid;

        // Call the create function using the SHORT STRINGS
        createScript.CallCreate();

        // Simulate the www response - assuming the SERVER WILL RETURN AN ERROR CODE (create.php 's echo message)
        string wwwResponse = "3: Invalid email format, the inputted username is not a valid email address."; // Expected error code since the username is not a valid email, and that error should trigger first

        yield return null; // wait for ONE frame to simulate the time for the create coroutine to start

        // Assert: Check if the correct failure message was logged
        LogAssert.Expect(LogType.Log, "User create FAILED. Error Code: " + wwwResponse); //CHECK IF FAILED in general, AND exactly why it failed (Username invalid error code SHOULD happen first)
        // Add more assertions as needed

        // Wait for the coroutine to complete
        yield return WaitForCoroutineCompletion(createScript);

        // Additional assertions related to UI changes or behaviors after successful create and login can be added here
    }
    [UnityTest]
    public IEnumerator ValidateCreate_inputShortInvalidU_withSamePasses_toolongP()  //With all 3 three fields being invalid, and the second password matching the first                           **** SIU T6
    {
        // Set up test data with INVALID INPUTS, AND MATCHED PASSWORDS that are too long
        createScript.c_username_email.text = shortUsernameInvalid;
        createScript.c_user_pass.text = toolongP;
        createScript.c_user_pass2.text = toolongP;

        // Call the create function using the SHORT STRINGS
        createScript.CallCreate();

        // Simulate the www response - assuming the SERVER WILL RETURN AN ERROR CODE (create.php 's echo message)
        string wwwResponse = "3: Invalid email format, the inputted username is not a valid email address."; // Expected error code since the username is not a valid email, and that error should trigger first

        yield return null; // wait for ONE frame to simulate the time for the create coroutine to start

        // Assert: Check if the correct failure message was logged
        LogAssert.Expect(LogType.Log, "User create FAILED. Error Code: " + wwwResponse); //CHECK IF FAILED in general, AND exactly why it failed (Username invalid error code SHOULD happen first)
        // Add more assertions as needed

        // Wait for the coroutine to complete
        yield return WaitForCoroutineCompletion(createScript);

        // Additional assertions related to UI changes or behaviors after successful create and login can be added here
    }
    //================================================================================= SHORT VALID USERNAME tests ===========================================================================================
    //6 different versions of the input to check, SHOULD ALL OUTPUT error code 10, since its the earliest error code that will be produced --the email isn't LONG ENOUGH in all 6 tests
    [UnityTest]
    public IEnumerator ValidateCreate_inputShortValidU_withDiffPasses_0ValidP() //With valid email but TOO SHORT, and all passes invalid, and the second password NOT matching the first                       **** SVU T1
    {
        // Set up test data with valid email format, but too short AND MISMATCHED PASSWORDS
        createScript.c_username_email.text = shortUsername;
        createScript.c_user_pass.text = shortPassword;
        createScript.c_user_pass2.text = secondPasswordInvalid;

        // Call the create function using the SHORT STRINGS
        createScript.CallCreate();

        // Simulate the www response - assuming the SERVER WILL RETURN AN ERROR CODE (create.php 's echo message)
        string wwwResponse = "10: The supplied username is not at least 10 characters in length, or it is more than 30."; // Expected error code since the username is valid email, BUT ITS NOT LONG ENOUGH, so that error should trigger first

        yield return null; // wait for ONE frame to simulate the time for the create coroutine to start

        // Assert: Check if the correct failure message was logged
        LogAssert.Expect(LogType.Log, "User create FAILED. Error Code: " + wwwResponse); //CHECK IF FAILED in general, AND exactly why it failed (Username too short error code SHOULD happen first)
        // Add more assertions as needed

        // Wait for the coroutine to complete
        yield return WaitForCoroutineCompletion(createScript);

        // Additional assertions related to UI changes or behaviors after successful create and login can be added here
    }
    [UnityTest]
    public IEnumerator ValidateCreate_inputShortValidU_withSamePasses_0ValidP() //With valid email but TOO SHORT, and all passes invalid, BUT MATCHING                                                            **** SVU T2
    {
        // Set up test data with valid email format, but too short AND MATCHED PASSWORDS
        createScript.c_username_email.text = shortUsername; //too short But valid email
        createScript.c_user_pass.text = shortPassword; //too short pass
        createScript.c_user_pass2.text = shortPassword; //same pass as above

        // Call the create function using the SHORT STRINGS
        createScript.CallCreate();

        // Simulate the www response - assuming the SERVER WILL RETURN AN ERROR CODE (create.php 's echo message)
        string wwwResponse = "10: The supplied username is not at least 10 characters in length, or it is more than 30."; // Expected error code since the username is valid email, BUT ITS NOT LONG ENOUGH, so that error should trigger first

        yield return null; // wait for ONE frame to simulate the time for the create coroutine to start

        // Assert: Check if the correct failure message was logged
        LogAssert.Expect(LogType.Log, "User create FAILED. Error Code: " + wwwResponse); //CHECK IF FAILED in general, AND exactly why it failed (Username length error code SHOULD happen first)
        // Add more assertions as needed

        // Wait for the coroutine to complete
        yield return WaitForCoroutineCompletion(createScript);

        // Additional assertions related to UI changes or behaviors after successful create and login can be added here
    }
    [UnityTest]
    public IEnumerator ValidateCreate_inputShortValidU_withDiffPasses_1ValidP()  //With 2 fields being invalid (user and 2nd pass), but DIFFERENT PASSES                                                     **** SVU T3
    {
        // Set up test data with INVALID INPUTS, BUT MISMATCHED PASSWORDS (and one is valid
        createScript.c_username_email.text = shortUsername; //too short But valid email
        createScript.c_user_pass.text = validPassword; //valid pass
        createScript.c_user_pass2.text = shortPassword; //mismatched 2nd pass that is also invalid

        // Call the create function using the SHORT STRINGS
        createScript.CallCreate();

        // Simulate the www response - assuming the SERVER WILL RETURN AN ERROR CODE (create.php 's echo message)
        string wwwResponse = "10: The supplied username is not at least 10 characters in length, or it is more than 30."; // Expected error code since the username is valid email, BUT ITS NOT LONG ENOUGH, so that error should trigger first

        yield return null; // wait for ONE frame to simulate the time for the create coroutine to start

        // Assert: Check if the correct failure message was logged
        LogAssert.Expect(LogType.Log, "User create FAILED. Error Code: " + wwwResponse); //CHECK IF FAILED in general, AND exactly why it failed (Username too short error code SHOULD happen first)
        // Add more assertions as needed

        /// Wait for the coroutine to complete
        yield return WaitForCoroutineCompletion(createScript);

        // Additional assertions related to UI changes or behaviors after successful create and login can be added here
    }
    [UnityTest]
    public IEnumerator ValidateCreate_inputShortValidU_withSamePasses_2ValidP()  //With 1 field being invalid (user), but MATCHING VALID PASSES                                                              **** SVU T4
    {
        // Set up test data with INVALID INPUTS, BUT MISMATCHED PASSWORDS (and one is valid)
        createScript.c_username_email.text = shortUsername; //too short But valid email
        createScript.c_user_pass.text = validPassword; //valid pass
        createScript.c_user_pass2.text = validPassword; //mismatched 2nd pass that is also invalid   *****THIS CREATED AN ACCOUNT SOMEHOW EVEN THOUGH it should trigger username length error

        // Call the create function using the SHORT STRINGS
        createScript.CallCreate();

        // Simulate the www response - assuming the SERVER WILL RETURN AN ERROR CODE (create.php 's echo message)
        string wwwResponse = "10: The supplied username is not at least 10 characters in length, or it is more than 30."; // Expected error code since the username is valid email, BUT ITS NOT LONG ENOUGH, so that error should trigger first

        yield return null; // wait for ONE frame to simulate the time for the create coroutine to start

        // Assert: Check if the correct failure message was logged
        LogAssert.Expect(LogType.Log, "User create FAILED. Error Code: " + wwwResponse); //CHECK IF FAILED in general, AND exactly why it failed (Username too short error code SHOULD happen first)
        // Add more assertions as needed

        // Wait for the coroutine to complete
        yield return WaitForCoroutineCompletion(createScript);

        // Additional assertions related to UI changes or behaviors after successful create and login can be added here
    }
    [UnityTest]
    public IEnumerator ValidateCreate_inputShortValidU_withDiffPasses_toolongP()  //With all 3 three fields being invalid, and the second password NOT matching the first                                     **** SVU T5
    {
        // Set up test data with INVALID INPUTS, AND MISMATCHED PASSWORDS that are too long
        createScript.c_username_email.text = shortUsername; //too short But valid email
        createScript.c_user_pass.text = toolongP;
        createScript.c_user_pass2.text = secondPasswordInvalid;

        // Call the create function using the SHORT STRINGS
        createScript.CallCreate();

        // Simulate the www response - assuming the SERVER WILL RETURN AN ERROR CODE (create.php 's echo message)
        string wwwResponse = "10: The supplied username is not at least 10 characters in length, or it is more than 30."; // Expected error code since the username is valid email, BUT ITS NOT LONG ENOUGH, so that error should trigger first

        yield return null; // wait for ONE frame to simulate the time for the create coroutine to start

        // Assert: Check if the correct failure message was logged
        LogAssert.Expect(LogType.Log, "User create FAILED. Error Code: " + wwwResponse); //CHECK IF FAILED in general, AND exactly why it failed (Username too short error code SHOULD happen first)
        // Add more assertions as needed

        // Wait for the coroutine to complete
        yield return WaitForCoroutineCompletion(createScript);

        // Additional assertions related to UI changes or behaviors after successful create and login can be added here
    }
    [UnityTest]
    public IEnumerator ValidateCreate_inputShortValidU_withSamePasses_toolongP()  //With all 3 three fields being invalid, and the second password matching the first                                           **** SVU T6
    {
        // Set up test data with INVALID INPUTS, AND MATCHED PASSWORDS that are too long
        createScript.c_username_email.text = shortUsername; //too short But valid email
        createScript.c_user_pass.text = toolongP;
        createScript.c_user_pass2.text = toolongP;

        // Call the create function using the SHORT STRINGS
        createScript.CallCreate();

        // Simulate the www response - assuming the SERVER WILL RETURN AN ERROR CODE (create.php 's echo message)
        string wwwResponse = "10: The supplied username is not at least 10 characters in length, or it is more than 30."; // Expected error code since the username is valid email, BUT ITS NOT LONG ENOUGH, so that error should trigger first

        yield return null; // wait for ONE frame to simulate the time for the create coroutine to start

        // Assert: Check if the correct failure message was logged
        LogAssert.Expect(LogType.Log, "User create FAILED. Error Code: " + wwwResponse); //CHECK IF FAILED in general, AND exactly why it failed (Username too short error code SHOULD happen first)
        // Add more assertions as needed

        // Wait for the coroutine to complete
        yield return WaitForCoroutineCompletion(createScript);

        // Additional assertions related to UI changes or behaviors after successful create and login can be added here
    }
    //================================================================================= EXISTING USERNAME tests ===========================================================================================
    //6 different versions of the input to check, SHOULD ALL OUTPUT error codes 8,9, OR 7 since its the email already exists (7), but the if the passwords dont match or are not the right length, they will hit errors 8 and 9.
    [UnityTest]
    public IEnumerator ValidateCreate_inputExistingUser_withDiffPasses_0ValidP() //With exisitng username, and all passes invalid, and the second password NOT matching the first                          **** EU T1
    {
        // Set up test data with existing email format, AND MISMATCHED invalid PASSWORDS
        createScript.c_username_email.text = existingUsername;//Existing valid email
        createScript.c_user_pass.text = shortPassword;
        createScript.c_user_pass2.text = secondPasswordInvalid;

        // Call the create function using the SHORT STRINGS
        createScript.CallCreate();

        // Simulate the www response - assuming the SERVER WILL RETURN AN ERROR CODE (create.php 's echo message)
        string wwwResponse = "8: The two supplied passwords do not match. Please try again."; // Expected error code, email already exists, but the passes aren't matching error triggers first

        yield return null; // wait for ONE frame to simulate the time for the create coroutine to start

        // Assert: Check if the correct failure message was logged
        LogAssert.Expect(LogType.Log, "User create FAILED. Error Code: " + wwwResponse); //CHECK IF FAILED in general, AND exactly why it failed (Passwords don't match error code SHOULD happen first)
        // Add more assertions as needed

        // Wait for the coroutine to complete
        yield return WaitForCoroutineCompletion(createScript);

        // Additional assertions related to UI changes or behaviors after successful create and login can be added here
    }
    [UnityTest]
    public IEnumerator ValidateCreate_inputExistingUser_withSamePasses_0ValidP() //With exisitng username, and all passes invalid, BUT MATCHING                                                             **** EU T2
    {
        // Set up test data with existing email AND MATCHED invalid PASSWORDS
        createScript.c_username_email.text = existingUsername; //Existing valid email
        createScript.c_user_pass.text = shortPassword; //too short pass
        createScript.c_user_pass2.text = shortPassword; //same pass as above

        // Call the create function using the SHORT STRINGS
        createScript.CallCreate();

        // Simulate the www response - assuming the SERVER WILL RETURN AN ERROR CODE (create.php 's echo message)
        string wwwResponse = "9: The supplied passwords are not at least 10 characters in length, or they are more than 30."; // Expected error code, email already exists, but the passes aren't correct length error triggers first

        yield return null; // wait for ONE frame to simulate the time for the create coroutine to start

        // Assert: Check if the correct failure message was logged
        LogAssert.Expect(LogType.Log, "User create FAILED. Error Code: " + wwwResponse); //CHECK IF FAILED in general, AND exactly why it failed (Passes length error code SHOULD happen first)
        // Add more assertions as needed

        // Wait for the coroutine to complete
        yield return WaitForCoroutineCompletion(createScript);

        // Additional assertions related to UI changes or behaviors after successful create and login can be added here
    }
    [UnityTest]
    public IEnumerator ValidateCreate_inputExistingUser_withDiffPasses_1ValidP()  //With exisitng username and VALID 1st pass, but DIFFERENT PASSES                                                            **** EU T3
    {
        // Set up test data with INVALID INPUTS -- Already existing User with existing passes
        createScript.c_username_email.text = existingUsername; //Existing valid email 
        createScript.c_user_pass.text = validPassword; //valid pass
        createScript.c_user_pass2.text = secondPasswordInvalid; //diff password thats invalid

        // Call the create function using the SHORT STRINGS
        createScript.CallCreate();

        // Simulate the www response - assuming the SERVER WILL RETURN AN ERROR CODE (create.php 's echo message)
        string wwwResponse = "8: The two supplied passwords do not match. Please try again."; // Expected error code since passes don't match which should trigger BEFORE "user already exists" error

        yield return null; // wait for ONE frame to simulate the time for the create coroutine to start

        // Assert: Check if the correct failure message was logged
        LogAssert.Expect(LogType.Log, "User create FAILED. Error Code: " + wwwResponse); //CHECK IF FAILED in general, AND EXACTLY WHY (passes don't match error code SHOULD happen first)
        // Add more assertions as needed

        // Wait for the coroutine to complete
        yield return WaitForCoroutineCompletion(createScript);

        // Additional assertions related to UI changes or behaviors after successful create and login can be added here
    }
    [UnityTest]
    public IEnumerator ValidateCreate_inputExistingUser_withSamePasses_2ValidP() //With exisitng username and VALID MATCHING PASSES                                                                             **** EU T4
    {
        // Set up test data with INVALID INPUTS -- Already existing User with valid passes
        createScript.c_username_email.text = existingUsername; //Existing valid email 
        createScript.c_user_pass.text = validPassword; //valid pass
        createScript.c_user_pass2.text = validPassword; //matching valid 2ND password

        // Call the create function using the SHORT STRINGS
        createScript.CallCreate();

        // Simulate the www response - assuming the SERVER WILL RETURN AN ERROR CODE (create.php 's echo message)
        string wwwResponse = "7: This user ALREADY exists!"; // Expected error code since username already associated with an account

        yield return null; // wait for ONE frame to simulate the time for the create coroutine to start

        // Assert: Check if the correct failure message was logged
        LogAssert.Expect(LogType.Log, "User create FAILED. Error Code: " + wwwResponse); //CHECK IF FAILED in general, AND EXACTLY WHY (user already exists error code SHOULD happen first)
        // Add more assertions as needed

        // Wait for the coroutine to complete
        yield return WaitForCoroutineCompletion(createScript);

        // Additional assertions related to UI changes or behaviors after successful create and login can be added here
    }
    [UnityTest]
    public IEnumerator ValidateCreate_inputExistingUser_withDiffPasses_toolongP()  //With all 3 three fields being invalid, and the second password NOT matching the first                                     **** EU T5
    {
        // Set up test data with INVALID INPUTS, AND MISMATCHED PASSWORDS that are too long
        createScript.c_username_email.text = existingUsername; //Existing valid email 
        createScript.c_user_pass.text = toolongP;
        createScript.c_user_pass2.text = secondPasswordInvalid;

        // Call the create function using the SHORT STRINGS
        createScript.CallCreate();

        // Simulate the www response - assuming the SERVER WILL RETURN AN ERROR CODE (create.php 's echo message)
        string wwwResponse = "8: The two supplied passwords do not match. Please try again."; // Expected error code since passes don't match which should trigger BEFORE "user already exists" error

        yield return null; // wait for ONE frame to simulate the time for the create coroutine to start

        // Assert: Check if the correct failure message was logged
        LogAssert.Expect(LogType.Log, "User create FAILED. Error Code: " + wwwResponse); //CHECK IF FAILED in general, AND exactly why it failed (Username too short error code SHOULD happen first)
        // Add more assertions as needed

        // Wait for the coroutine to complete
        yield return WaitForCoroutineCompletion(createScript);

        // Additional assertions related to UI changes or behaviors after successful create and login can be added here
    }
    [UnityTest]
    public IEnumerator ValidateCreate_inputExistingUser_withSamePasses_toolongP()  //With all 3 three fields being invalid, and the second password matching the first                                         **** EU T6
    {
        // Set up test data with INVALID INPUTS, AND MATCHED PASSWORDS that are too long
        createScript.c_username_email.text = existingUsername; //Existing valid email 
        createScript.c_user_pass.text = toolongP;
        createScript.c_user_pass2.text = toolongP;

        // Call the create function using the SHORT STRINGS
        createScript.CallCreate();

        // Simulate the www response - assuming the SERVER WILL RETURN AN ERROR CODE (create.php 's echo message)
        string wwwResponse = "9: The supplied passwords are not at least 10 characters in length, or they are more than 30."; // Expected error code, email already exists, but the passes aren't correct length error triggers first

        yield return null; // wait for ONE frame to simulate the time for the create coroutine to start

        // Assert: Check if the correct failure message was logged
        LogAssert.Expect(LogType.Log, "User create FAILED. Error Code: " + wwwResponse); //CHECK IF FAILED in general, AND exactly why it failed (Username too short error code SHOULD happen first)
        // Add more assertions as needed

        // Wait for the coroutine to complete
        yield return WaitForCoroutineCompletion(createScript);

        // Additional assertions related to UI changes or behaviors after successful create and login can be added here
    }
    //================================================================================= NEW VALID USERNAME tests ===========================================================================================
    //5 different versions of the input to check, SHOULD ALL OUTPUT error codes 8,9 since the email is valid and new, but the if the passwords dont match or are not the right length, they will hit errors 8 and 9.
    //saving valid and matching passwords test for final createtest at bottom of page

    [UnityTest]
    public IEnumerator ValidateCreate_inputValidUser_withDiffPasses_0ValidP() //With VALID NEW username, and all passes invalid, and the second password NOT matching the first                              **** VU T1
    {
        // Set up test data with INVALID INPUTS -- Valid User with invalid passes
        createScript.c_username_email.text = newUser; //NEW valid email 
        createScript.c_user_pass.text = shortPassword; //invalid pass
        createScript.c_user_pass2.text = secondPasswordInvalid; //diff invalid pass than above

        // Call the create function using the SHORT STRINGS
        createScript.CallCreate();

        // Simulate the www response - assuming the SERVER WILL RETURN AN ERROR CODE (create.php 's echo message)
        string wwwResponse = "8: The two supplied passwords do not match. Please try again."; // Expected error code since the username IS VALID, but PASSES don't match AND are invalid

        yield return null; // wait for ONE frame to simulate the time for the create coroutine to start

        // Assert: Check if the correct failure message was logged
        LogAssert.Expect(LogType.Log, "User create FAILED. Error Code: " + wwwResponse); //CHECK IF FAILED in general, AND EXACTLY WHY (mismatched passwords error code SHOULD happen first)
        // Add more assertions as needed

        // Wait for the coroutine to complete
        yield return WaitForCoroutineCompletion(createScript);

        // Additional assertions related to UI changes or behaviors after successful create and login can be added here
    }
    [UnityTest]
    public IEnumerator ValidateCreate_inputValidUser_withSamePasses_0ValidP() //With VALID username field, AND INVALID passes THAT MATCH                                                                     **** VU T2
    {
        // Set up test data with INVALID INPUTS -- Valid User with invalid passes
        createScript.c_username_email.text = newUser; //NEW valid email that doesn't exist yet
        createScript.c_user_pass.text = shortPassword; //invalid pass
        createScript.c_user_pass2.text = shortPassword; //same invalid pass than above

        // Call the create function using the SHORT STRINGS
        createScript.CallCreate();

        // Simulate the www response - assuming the SERVER WILL RETURN AN ERROR CODE (create.php 's echo message)
        string wwwResponse = "9: The supplied passwords are not at least 10 characters in length, or they are more than 30."; // Expected error code since the username IS VALID, and PASSES match BUT are invalid

        yield return null; // wait for ONE frame to simulate the time for the create coroutine to start

        // Assert: Check if the correct failure message was logged
        LogAssert.Expect(LogType.Log, "User create FAILED. Error Code: " + wwwResponse); //CHECK IF FAILED in general, AND EXACTLY WHY (password length error code SHOULD happen first)
        // Add more assertions as needed

        // Wait for the coroutine to complete
        yield return WaitForCoroutineCompletion(createScript);

        // Additional assertions related to UI changes or behaviors after successful create and login can be added here
    }
    [UnityTest]
    public IEnumerator ValidateCreate_inputValidUser_withDiffPasses_1ValidP()  //With VALID username field, and VALID 1st pass, but DIFFERENT PASSES                                                          **** VU T3
    {
        // Set up test data with INVALID INPUTS -- Already existing User with existing passes
        createScript.c_username_email.text = newUser; //NEW valid email that doesn't exist yet
        createScript.c_user_pass.text = validPassword; //valid pass
        createScript.c_user_pass2.text = secondPasswordInvalid; //diff password thats invalid

        // Call the create function using the SHORT STRINGS
        createScript.CallCreate();

        // Simulate the www response - assuming the SERVER WILL RETURN AN ERROR CODE (create.php 's echo message)
        string wwwResponse = "8: The two supplied passwords do not match. Please try again."; // Expected error code since passes don't match which should trigger BEFORE creating the account

        yield return null; // wait for ONE frame to simulate the time for the create coroutine to start

        // Assert: Check if the correct failure message was logged
        LogAssert.Expect(LogType.Log, "User create FAILED. Error Code: " + wwwResponse); //CHECK IF FAILED in general, AND EXACTLY WHY (passes don't match error code SHOULD happen first)
        // Add more assertions as needed

        // Wait for the coroutine to complete
        yield return WaitForCoroutineCompletion(createScript);

        // Additional assertions related to UI changes or behaviors after successful create and login can be added here
    }
    
    [UnityTest]
    public IEnumerator ValidateCreate_inputValidUser_withDiffPasses_toolongP()  //With VALID username field, invalid passes and the second password NOT matching the first                                     **** VU T5
    {
        // Set up test data with INVALID INPUTS, AND MISMATCHED PASSWORDS that are too long
        createScript.c_username_email.text = newUser; //NEW valid email that doesn't exist yet 
        createScript.c_user_pass.text = toolongP;
        createScript.c_user_pass2.text = secondPasswordInvalid;

        // Call the create function using the SHORT STRINGS
        createScript.CallCreate();

        // Simulate the www response - assuming the SERVER WILL RETURN AN ERROR CODE (create.php 's echo message)
        string wwwResponse = "8: The two supplied passwords do not match. Please try again."; // Expected error code since passes don't match which should trigger BEFORE "user already exists" error

        yield return null; // wait for ONE frame to simulate the time for the create coroutine to start

        // Assert: Check if the correct failure message was logged
        LogAssert.Expect(LogType.Log, "User create FAILED. Error Code: " + wwwResponse); //CHECK IF FAILED in general, AND exactly why it failed (Username too short error code SHOULD happen first)
        // Add more assertions as needed

        // Wait for the coroutine to complete
        yield return WaitForCoroutineCompletion(createScript);

        // Additional assertions related to UI changes or behaviors after successful create and login can be added here
    }
    [UnityTest]
    public IEnumerator ValidateCreate_inputValidUser_withSamePasses_toolongP()  ////With VALID username field, too long passes and the second password matching the first                                         **** VU T6
    {
        // Set up test data with INVALID INPUTS, AND MATCHED PASSWORDS that are too long
        createScript.c_username_email.text = newUser; //NEW valid email that doesn't exist yet
        createScript.c_user_pass.text = toolongP;
        createScript.c_user_pass2.text = toolongP;

        // Call the create function using the SHORT STRINGS
        createScript.CallCreate();

        // Simulate the www response - assuming the SERVER WILL RETURN AN ERROR CODE (create.php 's echo message)
        string wwwResponse = "9: The supplied passwords are not at least 10 characters in length, or they are more than 30."; // Expected error code, email already exists, but the passes aren't correct length error triggers first

        yield return null; // wait for ONE frame to simulate the time for the create coroutine to start

        // Assert: Check if the correct failure message was logged
        LogAssert.Expect(LogType.Log, "User create FAILED. Error Code: " + wwwResponse); //CHECK IF FAILED in general, AND exactly why it failed (Username too short error code SHOULD happen first)
        // Add more assertions as needed

        // Wait for the coroutine to complete
        yield return WaitForCoroutineCompletion(createScript);

        // Additional assertions related to UI changes or behaviors after successful create and login can be added here
    }



    /*//==================================================================================== VALID ACCOUNT tests ===========================================================================================
    // Test the submit button with VALID username and matching VALID passwords
    [UnityTest]
    public IEnumerator ValidateCreate_inputValidUandP_CreateNewUser() //With VALID username field, and VALID MATCHING PASSES                                                                   --Technically this is:  **** VU T4
    {
        // Set up test data with VALID INPUTS -- New User with valid passes
        createScript.c_username_email.text = newUser; //NEW valid email that doesn't exist yet
        ccreateScript.c_user_pass.text = validPassword; //valid pass
        createScript.c_user_pass2.text = validPassword; //matching valid 2ND password

        // Call the create function using the SHORT STRINGS
        createScript.CallCreate();

        // Simulate the www response - assuming the SERVER WILL RETURN SUCCESS CODE (create.php 's echo message)
        string todaysDate = System.DateTime.Now.ToString(\"yyyy-MM-dd");
        string wwwResponse = "0\t" + todaysDate +"\t0\t0\t0"; // Get success message from Create.php's echo inside of the ReadInputscript

        yield return null; // wait for ONE frame to simulate the time for the create coroutine to start

        // Assert: Check if the correct success message was logged
        LogAssert.Expect(LogType.Log, "User created and logged in successfully. Account Values: " + wwwResponse); //Entering valid and new username and matching passwords will generate new account and return success message
        // Add more assertions as needed

        // Wait for the coroutine to complete
        yield return WaitForCoroutineCompletion(createScript);

        // Additional assertions related to UI changes or behaviors after successful create and login can be added here
    }*/


    [TearDown]
    public void TearDown()
    {
        // Clean up all the created objects once the tests are done
        Object.DestroyImmediate(createScript.gameObject);
        Object.DestroyImmediate(usernameInputField.gameObject);
        Object.DestroyImmediate(passwordInputField.gameObject);
        Object.DestroyImmediate(passwordInputField2.gameObject);
    }

}
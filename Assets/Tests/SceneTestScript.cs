using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    /// <summary>
    /// This class is for unit test
    /// </summary>
    public class SceneTestScript
    {
        // A Test behaves as an ordinary method
        [Test]
        public void GameObject_CreatedWithGiven_WillHaveTheName()
        {
            
            var go = new GameObject("Player");
            Assert.AreEqual("Player1", go.name);
            // Use the Assert class to test conditions
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator NewTestScriptWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}

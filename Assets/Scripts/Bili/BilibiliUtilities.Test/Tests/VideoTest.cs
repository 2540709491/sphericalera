using System;
using BilibiliUtilities.Utils.CentralUtils;
using UnityEngine;

namespace BilibiliUtilities.Test.Tests
{
    public class VideoTest
    {

        public static async void TestAvToBv()
        {
            var BvId = await VideoUtil.AvToBv(170001);
            Debug.Log(BvId);
        }


        public static async void TestBvToAv()
        {
            var AvId = await VideoUtil.BvToAv("BV17x411w7KC");
            Debug.Log(AvId);
        }
    }
}
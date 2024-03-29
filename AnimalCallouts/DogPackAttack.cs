﻿using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using FivePD.API;
using FivePD.API.Utils;


namespace AnimalCallouts
{
    [CalloutProperties("Dog Pack Attack", "BGHDDevelopment", "1.1.0")]
    public class DogPackAttack : Callout
    {
        private string[] animalList = {"a_c_chop"};
        Ped victim;
        Ped animal, animal2, animal3;


        public DogPackAttack()
        {
            Random rnd = new Random();
            float offsetX = rnd.Next(100, 700);
            float offsetY = rnd.Next(100, 700);

            InitInfo(World.GetNextPositionOnStreet(Game.PlayerPed.GetOffsetPosition(new Vector3(offsetX, offsetY, 0))));
            ShortName = "Dog Pack Attack";
            CalloutDescription = "Someone is being attacked by a pack of dogs!";
            ResponseCode = 3;
            StartDistance = 150f;
        }

        private Blip animalBlip1, animalBlip2, animalBlip3, animalBlip4, victimBlip1;

        public async override void OnStart(Ped player)
        {
            base.OnStart(player);
            Random random = new Random();
            string animaltype = animalList[random.Next(animalList.Length)];
            PedHash Hash = (PedHash) API.GetHashKey(animaltype);
            animal = await SpawnPed(Hash, Location);
            animal2 = await SpawnPed(Hash, Location);
            animal3 = await SpawnPed(Hash, Location);
            victim = await SpawnPed(RandomUtils.GetRandomPed(), Location);
            API.SetAnimalMood(Hash.GetHashCode(), 1);
            animal.AlwaysKeepTask = true;
            animal.BlockPermanentEvents = true;
            animal2.AlwaysKeepTask = true;
            animal2.BlockPermanentEvents = true;
            animal3.AlwaysKeepTask = true;
            animal3.BlockPermanentEvents = true;
            victim.AlwaysKeepTask = true;
            victim.BlockPermanentEvents = true;
            Notify("~r~[AnimalCallouts] ~y~Victim is being attacked by a pack of dogs!");
            animal.AttachBlip();
            animal2.AttachBlip();
            animal3.AttachBlip();
            victim.AttachBlip();
            victim.Task.ReactAndFlee(animal);
            API.Wait(500);
            animal.Task.FightAgainst(victim);
            animal2.Task.FightAgainst(victim);
            animal3.Task.FightAgainst(victim);
            API.Wait(2000);
            animal3.Task.FightAgainst(player);
        }

        public async override Task OnAccept()
        {
            InitBlip();
            UpdateData();
        }

        private void Notify(string message)
        {
            API.BeginTextCommandThefeedPost("STRING");
            API.AddTextComponentSubstringPlayerName(message);
            API.EndTextCommandThefeedPostTicker(false, true);
        }
    }
}
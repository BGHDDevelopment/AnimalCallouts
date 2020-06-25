using System;
using System.Drawing;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using FivePD.API;

namespace AnimalCallouts
{
    [CalloutProperties("Mountain Lion Attack", "BGHDDevelopment", "1.0.5")]

    public class Attack : Callout
    {

        private string[] animalList = {"a_c_mtlion"};
        Ped victim;
        Ped animal;

        
        public Attack()
        {
            Random rnd = new Random();
            float offsetX = rnd.Next(100, 700);
            float offsetY = rnd.Next(100, 700);

            InitInfo(World.GetNextPositionOnStreet(Game.PlayerPed.GetOffsetPosition(new Vector3(offsetX, offsetY, 0))));
            ShortName = "Mountain Lion Attack";
            CalloutDescription = "Someone is being attacked by a mountain lion!";
            ResponseCode = 3;
            StartDistance = 150f;
        } 
        
        public override void OnStart(Ped player)
        {
            base.OnStart(player);
            animal.AttachBlip();
            victim.AttachBlip();
            victim.Task.ReactAndFlee(animal);
            API.Wait(500);
            animal.Task.FightAgainst(victim);
        }
        
        public async override Task OnAccept()
        {
            InitBlip();
            UpdateData();
            Random random = new Random();
            string animaltype = animalList[random.Next(animalList.Length)];
            PedHash Hash = (PedHash) API.GetHashKey(animaltype);
            animal = await SpawnPed(Hash, Location);
            victim = await SpawnPed(GetRandomPed(), Location);
            API.SetAnimalMood(Hash.GetHashCode(), 1);
            animal.AlwaysKeepTask = true;
            animal.BlockPermanentEvents = true;
            victim.AlwaysKeepTask = true;
            victim.BlockPermanentEvents = true;
            Notify("~r~[AnimalCallouts] ~y~Victim is being chased by a mountain lion!");

        }
        public override void OnCancelBefore()
        {
        }
        private void Notify(string message)
        {
            API.BeginTextCommandThefeedPost("STRING");
            API.AddTextComponentSubstringPlayerName(message);
            API.EndTextCommandThefeedPostTicker(false, true);
        }

    }
}
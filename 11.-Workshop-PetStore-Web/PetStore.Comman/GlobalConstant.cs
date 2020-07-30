using System;
using System.Collections.Generic;
using System.Text;

namespace PetStore.Comman
{
    public static class GlobalConstant
    {
        //Client
        public const int ClientPasswordMinLength = 5;
        public const int ClientPasswordMaxLength = 20;
        public const int ClientUsernameMinLength = 6;
        public const int ClientUsernameMaxLength = 20;
        public const int ClientEmailMaxLength = 25;

        //Order
        public const int OrderTownNameMaxLength = 30;
        public const int OrderAddressMaxLength = 100;

        //Pet
        public const int PetNameMinLength = 3;
        public const int PetNameMaxLength = 10;
        public const int MinimumPetAge = 0;
        public const int MaximumPetAge = 200;

        //Breed
        public const int BreedNameMinLength = 3;
        public const int BreedNameMaxLength = 30;

        //Product
        public const int ProductNameMinLength = 3;
        public const int ProductNameMaxLength = 30;
    }
}

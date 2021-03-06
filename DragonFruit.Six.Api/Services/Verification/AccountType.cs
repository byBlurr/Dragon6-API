﻿// Dragon6 API Copyright 2020 DragonFruit Network <inbox@dragonfruit.network>
// Licensed under Apache-2. Please refer to the LICENSE file for more info

namespace DragonFruit.Six.Api.Services.Verification
{
    public enum AccountType
    {
        Blocked = -1,

        Normal = 0,

        Beta = 1,
        Translator = 2,
        Verified = 3,
        Supporter = 4,
        Contributor = 5,

        Developer = 100
    }
}

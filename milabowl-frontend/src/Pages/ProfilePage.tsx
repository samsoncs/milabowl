import { Typography } from '@material-ui/core';
import React, { useEffect, useState } from 'react';
import { getAccessTokenFromCache, getDecodedToken } from '../Api/AuthService';
import { GetProfilePicture } from '../Api/api';

const ProfilePage = () => {
    const [preferredUserName, setPreferredUserName] = useState<string | undefined>();
    useEffect(() => {
        async function getProfile() {
            const accessToken = await getAccessTokenFromCache();
            const decodedToken = getDecodedToken(accessToken);
            setPreferredUserName(decodedToken?.preferred_username);
            // const profilePicture = await GetProfilePicture();
            // console.log(profilePicture);
        }

        getProfile();
    },
    []);
    return(
    <div>
        <Typography variant="h6">
            {preferredUserName}
        </Typography>
    </div>
)};

export default ProfilePage;

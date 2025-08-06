export const BOMB_EMOJIS = {
  dynamite: '🧨',
  bomb: '💣',
  nuke: '☢️',
  exploded: '💥',
  handed: '👋',
  collateral: '💀',
  thrown: '🤾',
  diffused: '💨',
  diffusalKit: '🧰',
  holding: '👐',
};

export const GetBombEmoji = (bombTier: string): string => {
  switch (bombTier) {
    case 'Dynamite':
      return BOMB_EMOJIS.dynamite;
    case 'Bomb':
      return BOMB_EMOJIS.bomb;
    case 'Nuke':
      return BOMB_EMOJIS.nuke;
    default:
      return '';
  }
};

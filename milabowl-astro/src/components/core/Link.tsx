export interface LinkProps {
  href: string;
  disableUnderline?: boolean;
  children: React.ReactNode;
}

const Link: React.FC<LinkProps> = ({ href, disableUnderline, children }) => (
  <a
    className={['text-primary', disableUnderline ? '' : 'underline'].join(' ')}
    href={href}
  >
    {children}
  </a>
);

export default Link;

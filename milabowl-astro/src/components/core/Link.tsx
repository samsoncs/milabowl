export interface LinkProps {
    href: string
    disableUnderline?: boolean
    children: React.ReactNode
}

const Link: React.FC<LinkProps> = ({ href, disableUnderline, children }) => (
    <a
        className={[
            'text-indigo-900',
            disableUnderline ? '' : 'underline',
        ].join(' ')}
        href={href}
    >
        {children}
    </a>
)

export default Link

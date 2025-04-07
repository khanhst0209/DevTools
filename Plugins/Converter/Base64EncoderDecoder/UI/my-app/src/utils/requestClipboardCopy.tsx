/**
 * Sends a clipboard copy request from the iframe to the host.
 * The host must handle the message and call `navigator.clipboard.writeText()`.
 *
 * @param text The text you want the host to copy to the user's clipboard
 */
export function requestClipboardCopy(text: string): void {
  const message = {
    type: "copyToClipboard",
    payload: { text },
  };

  console.log("📤 iframe requesting clipboard copy:", message);
  window.parent.postMessage(message, "*"); // Replace * with host origin in production
}

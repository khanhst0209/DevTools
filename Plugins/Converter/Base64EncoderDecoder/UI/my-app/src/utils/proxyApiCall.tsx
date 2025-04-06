/**
 * Represents the payload structure sent from the iframe to the host.
 */
export type ToolApiRequest = {
  /** Relative API endpoint (e.g. "/hash", "/compare") */
  endpoint: string;

  /** HTTP method to use for the request */
  method: "GET" | "POST" | "PUT" | "DELETE";

  /** Optional request body */
  body?: any;

  /** Id of the tool (used by the host to identify the plugin); defaults to `window.name` */
  toolId?: string;
};

/**
 * Message structure sent from the iframe to the host via `postMessage`.
 */
export type ToolApiMessage = {
  type: "toolApiRequest";
  requestId: string;
  payload: ToolApiRequest;
};

/**
 * Message structure sent from the host back to the iframe via `postMessage`.
 */
export type ToolApiResponse = {
  type: "toolApiResponse";
  requestId: string;
  payload: any;
};

/**
 * Sends an API request from a plugin iframe to its parent host page, which performs
 * the actual authenticated API call on behalf of the tool and returns the result.
 *
 * This enables secure cross-origin API communication without exposing authentication
 * tokens to the iframe.
 *
 * @template T The expected shape of the response payload
 * @param endpoint Relative API endpoint (e.g. "/hash", "/compare")
 * @param method HTTP method to use (GET, POST, etc.)
 * @param body Optional request body to send (will be JSON.stringified)
 * @returns Promise resolving to the response payload of type T
 *
 * @example
 * const result = await proxyApiCall<{ hash: string }>("/hash", "POST", {
 *   text: "Hello world",
 *   salt: 10,
 * });
 * console.log(result.hash); // => "hashed_value"
 */
export function proxyApiCall<T = any>(
  endpoint: string,
  method: ToolApiRequest["method"],
  body?: any
): Promise<T> {
  return new Promise((resolve) => {
    const requestId = crypto.randomUUID();

    const handleResponse = (event: MessageEvent<ToolApiResponse>) => {
      if (
        event.data.type === "toolApiResponse" &&
        event.data.requestId === requestId
      ) {
        window.removeEventListener("message", handleResponse);
        console.log("📥 iframe received response:", event.data.payload);
        resolve(event.data.payload);
      }
    };

    window.addEventListener("message", handleResponse);

    const message: ToolApiMessage = {
      type: "toolApiRequest",
      requestId,
      payload: {
        endpoint,
        method,
        body,
        toolId: window.name,
      },
    };

    console.log("📤 iframe sending to host:", message);
    window.parent.postMessage(message, "*");
  });
}
